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

		public LoginService(IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository, IHandRepository handRepository)
		{
			_playerInGameRepository = playerInGameRepository;
			_playerRepository = playerRepository;
			_handRepository = handRepository;

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

				await _playerInGameRepository.RemoveAll(human.Id);

				foreach (var bot in bots)
				{
					await _playerInGameRepository.AddPlayer(bot.Id, human.Id);
					logger.Info(StringHelper.BotJoinGame(bot.Id, human.Id));
				}

				await _playerInGameRepository.AddHuman(human.Id);
				logger.Info(StringHelper.HumanJoinGame(human.Id, human.Id));

				await _handRepository.RemoveAll(human.Id);
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
				var playerIsInGame = await _playerInGameRepository.IsInGame(player.Id, player.Id);

				if(!playerIsInGame)
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
