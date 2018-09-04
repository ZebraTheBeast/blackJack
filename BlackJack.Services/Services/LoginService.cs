using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.DataAccess.Interfaces;
using System;
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

		public LoginService(IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository, IHandRepository handRepository, IGameRepository gameRepository)
		{
			_playerInGameRepository = playerInGameRepository;
			_playerRepository = playerRepository;
			_handRepository = handRepository;
			_gameRepository = gameRepository;

			var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
			NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
		}

		public async Task<int> StartGame(string playerName, int botsAmount)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				if (playerName == Constant.DealerName)
				{
					throw new Exception(StringHelper.NotAvailibleName());
				}

				var human = await _playerRepository.GetByName(playerName);
				var bots = await _playerRepository.GetBots(playerName, botsAmount);
				var oldGameId = await _gameRepository.GetGameIdByHumanId(human.Id);

				await _playerInGameRepository.RemoveAll(oldGameId);
				if (oldGameId != 0)
				{
					await _gameRepository.Delete(oldGameId);
				}

				await _gameRepository.Create(human.Id);

				var gameId = await _gameRepository.GetGameIdByHumanId(human.Id);

				foreach (var bot in bots)
				{
					await _playerInGameRepository.AddPlayer(bot.Id, gameId);
					logger.Info(StringHelper.BotJoinGame(bot.Id, human.Id));
				}

				await _playerInGameRepository.AddPlayer(human.Id, gameId);
				logger.Info(StringHelper.HumanJoinGame(human.Id, gameId));

				await _handRepository.RemoveAll(gameId);
				return human.Id;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<int> LoadGame(string playerName)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				if (playerName == Constant.DealerName)
				{
					throw new Exception(StringHelper.NotAvailibleName());
				}

				var player = await _playerRepository.GetByName(playerName);
				var gameId = await _gameRepository.GetGameIdByHumanId(player.Id);

				var playerIsInGame = await _playerInGameRepository.IsInGame(player.Id, gameId);

				if (!playerIsInGame)
				{
					throw new Exception(StringHelper.NoLastGame());
				}

				return player.Id;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}
	}
}
