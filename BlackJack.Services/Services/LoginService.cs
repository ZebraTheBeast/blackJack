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
				var oldGame = await _gameRepository.GetGameByHumanId(human.Id);
				
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
					logger.Log(LogHelper.GetEvent(bot.Id, game.Id, StringHelper.BotJoinGame()));
				}

				await _playerInGameRepository.AddPlayer(human.Id, game.Id);
				logger.Log(LogHelper.GetEvent(human.Id, game.Id, StringHelper.HumanJoinGame()));

				
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
				var game = await _gameRepository.GetGameByHumanId(player.Id);

				var playerIsInGame = await _playerInGameRepository.IsInGame(player.Id, game.Id);

				if (!playerIsInGame)
				{
					throw new Exception(StringHelper.NoLastGame());
				}

				logger.Log(LogHelper.GetEvent(player.Id, game.Id, StringHelper.PlayerContinueGame()));
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
