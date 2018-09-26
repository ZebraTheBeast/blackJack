﻿using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using NLog;
using System;
using System.Collections.Generic;
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

		private Logger _logger;

		public LoginService(ICardProvider cardProvider, IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository, IHandRepository handRepository, IGameRepository gameRepository)
		{
			var path = string.Empty;

			_playerInGameRepository = playerInGameRepository;
			_playerRepository = playerRepository;
			_handRepository = handRepository;
			_gameRepository = gameRepository;

			_cardProvider = cardProvider;

			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<int> StartGame(string playerName, int botsAmount)
		{
			if (playerName == Constant.DealerName)
			{
				throw new Exception(StringHelper.NotAvailibleName());
			}

			Player human = await _playerRepository.GetPlayerByName(playerName);

			if (human == null)
			{
				var player = new Player { Name = playerName };
				await _playerRepository.CreateNewPlayer(player);
				human = await _playerRepository.GetPlayerByName(playerName);
			}

			if (human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.RestorePlayerPoints(human.Id);
				human.Points = Constant.DefaultPointsValue;
			}

			List<Player> bots = await _playerRepository.GetBots(playerName, botsAmount);
			var oldGame = await _gameRepository.GetGameIdByHumanId(human.Id);
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
				await _playerRepository.RestorePlayersPoints(playersIdWithoutPoints);
			}

			await _cardProvider.RestoreCardsInDb();

			if (oldGame != 0)
			{
				await _handRepository.RemoveAllCardsInHand(oldGame);
				await _playerInGameRepository.RemoveAllPlayersFromGame(oldGame);
				await _gameRepository.DeleteGameById(oldGame);
			}

			int gameId = await _gameRepository.StartNewGame();

			foreach (var bot in bots)
			{
				await _playerInGameRepository.AddPlayerToGame(bot.Id, gameId, false);
				_logger.Log(LogHelper.GetEvent(bot.Id, gameId, StringHelper.BotJoinGame()));
			}

			await _playerInGameRepository.AddPlayerToGame(human.Id, gameId, true);
			_logger.Log(LogHelper.GetEvent(human.Id, gameId, StringHelper.HumanJoinGame()));

			return gameId;
		}

		public async Task<int> LoadGame(string playerName)
		{
			if (playerName == Constant.DealerName)
			{
				throw new Exception(StringHelper.NotAvailibleName());
			}

			Player player = await _playerRepository.GetPlayerByName(playerName);

			if (player == null)
			{
				var newPlayer = new Player { Name = playerName };
				await _playerRepository.CreateNewPlayer(newPlayer);
				player = await _playerRepository.GetPlayerByName(playerName);
			}

			if (player.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.RestorePlayerPoints(player.Id);
				player.Points = Constant.DefaultPointsValue;
			}

			var gameId = await _gameRepository.GetGameIdByHumanId(player.Id);

			await _cardProvider.RestoreCardsInDb();

			if (gameId == 0)
			{
				throw new Exception(StringHelper.NoLastGame());
			}

			_logger.Log(LogHelper.GetEvent(player.Id, gameId, StringHelper.PlayerContinueGame()));
			return gameId;
		}
	}
}
