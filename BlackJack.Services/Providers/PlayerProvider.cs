using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
using BlackJack.DataAccess.Interfaces;
using BlackJack.BusinessLogic.Helper;
using BlackJack.Configurations;

namespace BlackJack.BusinessLogic.Providers
{
	public class PlayerProvider : IPlayerProvider
	{
		IPlayerRepository _playerRepository;


		public PlayerProvider(IPlayerRepository playerRepository)
		{
			_playerRepository = playerRepository;
		}

		public async Task<List<PlayerViewModel>> GetBotsInfo(IEnumerable<int> botsIdList)
		{
			var playerViewModelList = new List<PlayerViewModel>();

			foreach (var botId in botsIdList)
			{
				var player = await GetPlayerInfo(botId);
				playerViewModelList.Add(player);
			}

			return playerViewModelList;
		}
		
		public async Task<int> GetIdByName(string name)
		{
			var player = await _playerRepository.GetByName(name);
			return player.Id;
		}

		public async Task<PlayerViewModel> GetPlayerInfo(int playerId)
		{

			var player = await _playerRepository.GetById(playerId);

			var playerViewModel = new PlayerViewModel
			{
				Id = player.Id,
				Name = player.Name,
				Points = player.Points,
				Hand = new HandViewModel
				{
					CardList = new List<CardViewModel>()
				}
			};

			return playerViewModel;
		}

		public async Task<DealerViewModel> GetDealer(int gameId)
		{
			var dealer = await _playerRepository.GetByName(Configurations.Constant.DealerName);

			var dealerViewModel = new DealerViewModel
			{
				Id = dealer.Id,
				Name = dealer.Name
			};

			return dealerViewModel;
		}

		public async Task<string> UpdateScore(int playerId, int playerBetValue, int playerCardsValue, int dealerCardsValue, int gameId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Info(StringHelper.PlayerValue(playerId, playerCardsValue, dealerCardsValue, gameId));

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
				logger.Info(StringHelper.PlayerDraw(playerId, gameId));
				return OptionHelper.OptionDraw();
			}

			return null;
		}

		private async Task PlayerLosePoints(int playerId, int gameId, int playerBetValue)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Info(StringHelper.PlayerLose(playerId, playerBetValue, gameId));

			var player = await GetPlayerInfo(playerId);
			var newPointsValue = player.Points - playerBetValue;
			await _playerRepository.UpdatePoints(playerId, newPointsValue);
		}

		private async Task PlayerWinPoints(int playerId, int gameId, int playerBetValue)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			logger.Info(StringHelper.PlayerWin(playerId, playerBetValue, gameId));

			var player = await GetPlayerInfo(playerId);
			var newPointsValue = player.Points + playerBetValue;
			await _playerRepository.UpdatePoints(playerId, newPointsValue);
		}

	}
}
