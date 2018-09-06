using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.DataAccess.Interfaces;
using BlackJack.BusinessLogic.Helper;
using BlackJack.Configurations;
using BlackJack.Entities;
using BlackJack.BusinessLogic.Helpers;

namespace BlackJack.BusinessLogic.Providers
{
	public class PlayerProvider : IPlayerProvider
	{
		IPlayerRepository _playerRepository;


		public PlayerProvider(IPlayerRepository playerRepository)
		{
			_playerRepository = playerRepository;
		}

		public async Task<List<Player>> GetBotsInfo(IEnumerable<int> botsIdList)
		{
			var playersList = new List<Player>();

			foreach (var botId in botsIdList)
			{
				var player = await GetPlayerById(botId);
				playersList.Add(player);
			}

			return playersList;
		}
		
		public async Task<int> GetIdByName(string name)
		{
			var player = await _playerRepository.GetByName(name);
			return player.Id;
		}

		public async Task<Player> GetPlayerById(int playerId)
		{
			var player = await _playerRepository.GetById(playerId);
			return player;
		}

		public async Task<Player> GetDealer(int gameId)
		{
			var dealer = await _playerRepository.GetByName(Configurations.Constant.DealerName);
			return dealer;
		}

		public async Task<string> UpdateScore(int playerId, int playerBetValue, int playerCardsValue, int dealerCardsValue, int gameId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerValue(playerCardsValue, dealerCardsValue)));

			if ((playerCardsValue > dealerCardsValue)
			&& (playerCardsValue <= Constant.WinValue))
			{
				await PlayerWinPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionWin();
			}

			if ((playerCardsValue <= Constant.WinValue)
			&& (dealerCardsValue > Constant.WinValue))
			{
				await PlayerWinPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionWin();
			}

			if (playerCardsValue > Constant.WinValue)
			{
				await PlayerLosePoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionLose();
			}

			if ((dealerCardsValue > playerCardsValue)
			&& (dealerCardsValue <= Constant.WinValue))
			{
				await PlayerLosePoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionLose();
			}

			if ((dealerCardsValue == playerCardsValue)
			&& (playerCardsValue <= Constant.WinValue))
			{
				logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDraw()));
				return OptionHelper.OptionDraw();
			}

			return null;
		}

		private async Task PlayerLosePoints(int playerId, int gameId, int playerBetValue)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerLose(playerBetValue)));

			var player = await GetPlayerById(playerId);
			var newPointsValue = player.Points - playerBetValue;
			await _playerRepository.UpdatePoints(playerId, newPointsValue);
		}

		private async Task PlayerWinPoints(int playerId, int gameId, int playerBetValue)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerWin(playerBetValue)));

			var player = await GetPlayerById(playerId);
			var newPointsValue = player.Points + playerBetValue;
			await _playerRepository.UpdatePoints(playerId, newPointsValue);
		}

	}
}
