using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using System.IO;
using BlackJack.BLL.Helper;

namespace BlackJack.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        IPlayerRepository _playerRepository;
        IPlayerInGameRepository _playerInGameRepository;
        IHandService _handService;

        public PlayerService(IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository, IHandService handService)
        {
            _playerRepository = playerRepository;
            _playerInGameRepository = playerInGameRepository;
            _handService = handService;
        }

        public async Task<List<PlayerViewModel>> GetBotsInGame(int gameId)
        {
            var botsIdList = await _playerInGameRepository.GetBots(gameId);
            var playerViewModelList = new List<PlayerViewModel>();
            foreach (var playerId in botsIdList)
            {
                var player = await _playerRepository.GetById(playerId);

                var playerViewModel = new PlayerViewModel()
                {
                    Hand = new HandViewModel
                    {
                        CardList = new List<CardViewModel>()
                    }
                };

                playerViewModel.Id = player.Id;
                playerViewModel.Name = player.Name;
                playerViewModel.Points = player.Points;
                playerViewModel.Hand = await _handService.GetPlayerHand(player.Id, gameId);

                playerViewModelList.Add(playerViewModel);
            }

            return playerViewModelList;
        }

        public async Task<int> GetIdByName(string name)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var player = await _playerRepository.GetByName(name);
                return player.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<int> SetPlayerToGame(string playerName)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var player = await _playerRepository.GetByName(playerName);
                var bots = await _playerRepository.GetBots();

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
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<int>> GetPlayersIdInGame(int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var playerIdList = await _playerInGameRepository.GetAll(gameId);

                if (playerIdList.Count() == 0)
                {
                    throw new Exception(StringHelper.NoPlayersInGame());
                }

                return playerIdList;
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task PlaceBet(int playerId, int betValue, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
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
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<PlayerViewModel> GetHumanInGame(int humanId)
        {
            try
            {
                var player = await _playerRepository.GetById(humanId);

                var playerViewModel = new PlayerViewModel
                {
                    Hand = new HandViewModel
                    {
                        CardList = new List<CardViewModel>()
                    }
                };
                playerViewModel.Id = player.Id;
                playerViewModel.Name = player.Name;
                playerViewModel.Points = player.Points;
                playerViewModel.Hand = await _handService.GetPlayerHand(player.Id, player.Id);

                return playerViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<DealerViewModel> GetDealer(int gameId)
        {
            try
            {
                var dealer = await _playerRepository.GetByName("Dealer");

                var dealerViewModel = new DealerViewModel
                {
                    Hand = new HandViewModel()
                    {
                        CardList = new List<CardViewModel>()
                    }
                };

                dealerViewModel.Id = dealer.Id;
                dealerViewModel.Name = dealer.Name;
                dealerViewModel.Hand = await _handService.GetPlayerHand(dealer.Id, gameId);

                return dealerViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


    }
}
