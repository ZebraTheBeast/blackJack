using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
using BlackJack.DataAccess.Interfaces;
using BlackJack.BusinessLogic.Helper;

namespace BlackJack.BusinessLogic.Providers
{
	public class PlayerProvider : IPlayerProvider
	{
		IPlayerRepository _playerRepository;
		IPlayerInGameRepository _playerInGameRepository;
		IHandProvider _handService;

		public PlayerProvider(IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository, IHandProvider handService)
		{
			_playerRepository = playerRepository;
			_playerInGameRepository = playerInGameRepository;
			_handService = handService;
		}

		public async Task<List<PlayerViewModel>> GetBotsInGame(int gameId)
		{
			var playerViewModelList = new List<PlayerViewModel>();
			var botsIdList = await _playerInGameRepository.GetBotsInGame(gameId);

			foreach (var playerId in botsIdList)
			{
				var player = await _playerRepository.GetById(playerId);

				var playerViewModel = new PlayerViewModel
				{
					Id = player.Id,
					Name = player.Name,
					Points = player.Points,
					Hand = await _handService.GetPlayerHand(player.Id, gameId)
				};

				playerViewModelList.Add(playerViewModel);
			}

			return playerViewModelList;
		}

		public async Task<int> GetIdByName(string name)
		{
			var player = await _playerRepository.GetByName(name);
			return player.Id;
		}

		public async Task<int> SetPlayerToGame(string playerName, int botsAmount)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();

			var player = await _playerRepository.GetByName(playerName);
			var bots = await _playerRepository.GetBots(playerName, botsAmount);

			await _playerInGameRepository.RemoveAll(player.Id);

			foreach (var bot in bots)
			{
				await _playerInGameRepository.AddPlayer(bot.Id, player.Id);
				logger.Info(StringHelper.BotJoinGame(bot.Id, player.Id));
			}

			await _playerInGameRepository.AddHuman(player.Id);
			logger.Info(StringHelper.HumanJoinGame(player.Id, player.Id));
			return player.Id;
		}

		public async Task<IEnumerable<int>> GetPlayersIdInGame(int gameId)
		{
			var playerIdList = await _playerInGameRepository.GetAll(gameId);

			if (playerIdList.Count() == 0)
			{
				throw new Exception(StringHelper.NoPlayersInGame());
			}

			return playerIdList;
		}

		public async Task PlaceBet(int playerId, int betValue, int gameId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();

			var player = await _playerRepository.GetById(playerId);

			if (player.Points < betValue)
			{
				throw new Exception(StringHelper.NotEnoughPoints(playerId, betValue));
			}

			if (betValue <= 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}

			await _playerInGameRepository.PlaceBet(playerId, betValue, gameId);

			logger.Info(StringHelper.PlayerPlaceBet(playerId, betValue, gameId));
		}

		public async Task<PlayerViewModel> GetHumanInGame(int humanId)
		{
			if (!(await _playerInGameRepository.IsInGame(humanId, humanId)))
			{
				throw new Exception(StringHelper.NoLastGame());
			}

			var player = await _playerRepository.GetById(humanId);

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

			playerViewModel.Hand = await _handService.GetPlayerHand(player.Id, player.Id);

			return playerViewModel;
		}

		public async Task<DealerViewModel> GetDealer(int gameId)
		{
			var dealer = await _playerRepository.GetByName(Configurations.Constant.DealerName);

			var dealerViewModel = new DealerViewModel
			{
				Id = dealer.Id,
				Name = dealer.Name,
				Hand = await _handService.GetPlayerHand(dealer.Id, gameId)
			};

			return dealerViewModel;
		}


	}
}
