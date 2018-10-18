using BlackJack.BusinessLogic.Helpers;
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
		private ICardInHandRepository _cardInHandRepository;
		private IGameRepository _gameRepository;

		private ICardProvider _cardProvider;

		private Logger _logger;

		public LoginService(ICardProvider cardProvider, IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository, ICardInHandRepository handRepository, IGameRepository gameRepository)
		{
			_playerInGameRepository = playerInGameRepository;
			_playerRepository = playerRepository;
			_cardInHandRepository = handRepository;
			_gameRepository = gameRepository;

			_cardProvider = cardProvider;

			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<long> StartGame(string playerName, int botsAmount)
		{
			Player human = await _playerRepository.GetPlayerByName(playerName);

			if (human == null)
			{
				var player = new Player { Name = playerName, Type = Entities.Enums.PlayerType.Human };
				await _playerRepository.Add(player);
				human = await _playerRepository.GetPlayerByName(playerName);
			}

			if (human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.UpdatePlayersPoints(new List<long> { human.Id }, Constant.DefaultPointsValue);
				human.Points = Constant.DefaultPointsValue;
			}

			List<Player> bots = await _playerRepository.GetBots(botsAmount);
            var dealer = await _playerRepository.GetDealer();
            bots.Add(dealer);

			var oldGameId = await _gameRepository.GetGameIdByHumanId(human.Id);
			var playersIdWithoutPoints = new List<long>();

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
				await _playerRepository.UpdatePlayersPoints(playersIdWithoutPoints, Constant.DefaultPointsValue);
			}

			await _cardProvider.RestoreCardsInDb();

			if (oldGameId != 0)
			{
				await _cardInHandRepository.RemoveAllCardsByGameId(oldGameId);
				await _playerInGameRepository.RemoveAllPlayersFromGame(oldGameId);
				await _gameRepository.DeleteGameById(oldGameId);
			}

			long gameId = await _gameRepository.Add(new Game());

			foreach (var bot in bots)
			{
				var botInGame = new PlayerInGame() { PlayerId = bot.Id, GameId = gameId, IsHuman = false };
				await _playerInGameRepository.Add(botInGame);
				_logger.Log(LogHelper.GetEvent(bot.Id, gameId, StringHelper.BotJoinGame));
			}

			var humanInGame = new PlayerInGame() { PlayerId = human.Id, GameId = gameId, IsHuman = true };
			await _playerInGameRepository.Add(humanInGame);
			_logger.Log(LogHelper.GetEvent(human.Id, gameId, StringHelper.HumanJoinGame));

			return gameId;
		}

		public async Task<long> LoadGame(string playerName)
		{
			Player player = await _playerRepository.GetPlayerByName(playerName);

			if (player == null)
			{
				throw new Exception(StringHelper.NoLastGame);
			}

			if (player.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.UpdatePlayersPoints(new List<long> { player.Id }, Constant.DefaultPointsValue);
				player.Points = Constant.DefaultPointsValue;
			}

			var gameId = await _gameRepository.GetGameIdByHumanId(player.Id);

			await _cardProvider.RestoreCardsInDb();

			if (gameId == 0)
			{
				throw new Exception(StringHelper.NoLastGame);
			}

			_logger.Log(LogHelper.GetEvent(player.Id, gameId, StringHelper.PlayerContinueGame));
			return gameId;
		}
	}
}
