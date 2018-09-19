﻿using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Services
{
	public class LoginService : ILoginService
	{
		private IPlayerInGameRepository _playerInGameRepository;
		private IPlayerRepository _playerRepository;
		private IHandRepository _handRepository;
		private IGameRepository _gameRepository;

		private ICardProvider _cardProvider;

		Logger _logger;

		public LoginService(ICardProvider cardProvider, IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository, IHandRepository handRepository, IGameRepository gameRepository)
		{
			var path = string.Empty;

			_playerInGameRepository = playerInGameRepository;
			_playerRepository = playerRepository;
			_handRepository = handRepository;
			_gameRepository = gameRepository;

			_cardProvider = cardProvider;

			path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
			LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<int> StartGame(string playerName, int botsAmount)
		{
			
			try
			{
				if (playerName == Constant.DealerName)
				{
					throw new Exception(StringHelper.NotAvailibleName());
				}

				Player human = await _playerRepository.GetByName(playerName);			

				if (human == null)
				{
					var player = new Player { Name = playerName };
					await _playerRepository.Create(player);
					human = await _playerRepository.GetByName(playerName);
				}

				if (human.Points <= Constant.MinPointsValueToPlay)
				{
					await _playerRepository.RestorePoints(human.Id);
					human.Points = Constant.DefaultPointsValue;
				}

				List<Player> bots = await _playerRepository.GetBots(playerName, botsAmount);
				Game oldGame = await _gameRepository.GetGameByHumanId(human.Id);
				var playersIdWithoutPoints = new List<int>();

				foreach (var bot in bots)
				{
					if (bot.Points < Constant.MinPointsValueToPlay)
					{
						playersIdWithoutPoints.Add(bot.Id);
						bot.Points = Constant.DefaultPointsValue;
					}
				}

				if (playersIdWithoutPoints.Count != 0)
				{
					await _playerRepository.RestorePoints(playersIdWithoutPoints);
				}	

				await _cardProvider.CheckDeck();

				if (oldGame != null)
				{
					await _handRepository.RemoveAll(oldGame.Id);
					await _playerInGameRepository.RemoveAll(oldGame.Id);
					await _gameRepository.Delete(oldGame.Id);
				}

				await _gameRepository.Create(human.Id);

				var game = await _gameRepository.GetGameByHumanId(human.Id);

				foreach (var bot in bots)
				{
					await _playerInGameRepository.AddPlayer(bot.Id, game.Id);
					_logger.Log(LogHelper.GetEvent(bot.Id, game.Id, StringHelper.BotJoinGame()));
				}

				await _playerInGameRepository.AddPlayer(human.Id, game.Id);
				_logger.Log(LogHelper.GetEvent(human.Id, game.Id, StringHelper.HumanJoinGame()));

				return game.Id;
			}
			catch (Exception exception)
			{
				_logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<int> LoadGame(string playerName)
		{
			try
			{
				if (playerName == Constant.DealerName)
				{
					throw new Exception(StringHelper.NotAvailibleName());
				}

				Player player = await _playerRepository.GetByName(playerName);

				if (player == null)
				{
					var newPlayer = new Player { Name = playerName };
					await _playerRepository.Create(newPlayer);
					player = await _playerRepository.GetByName(playerName);
				}

				if (player.Points <= Constant.MinPointsValueToPlay)
				{
					await _playerRepository.RestorePoints(player.Id);
					player.Points = Constant.DefaultPointsValue;
				}

				Game game = await _gameRepository.GetGameByHumanId(player.Id);

				await _cardProvider.CheckDeck();

				if (game == null)
				{
					throw new Exception(StringHelper.NoLastGame());
				}

				_logger.Log(LogHelper.GetEvent(player.Id, game.Id, StringHelper.PlayerContinueGame()));
				return game.Id;
			}
			catch (Exception exception)
			{
				_logger.Error(exception.Message);
				throw exception;
			}
		}
	}
}
