using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Services
{
	public class LoginService : ILoginService
	{
		private IPlayerProvider _playerProvider;
		private IHandProvider _handProvider;

		public LoginService(IPlayerProvider playerProvider, IHandProvider handProvider)
		{
			_playerProvider = playerProvider;
			_handProvider = handProvider;

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

				var humanId = await _playerProvider.SetPlayerToGame(playerName, botsAmount);
				await _handProvider.RemoveAllCardsInHand(humanId);
				return humanId;
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

				var id = await _playerProvider.GetIdByName(playerName);
				var human = await _playerProvider.GetHumanInGame(id);

				return id;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}
	}
}
