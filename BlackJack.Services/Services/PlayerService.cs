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

        public async Task<List<PlayerViewModel>> GetBotsInGame()
        {
            var botsIdList = await _playerInGameRepository.GetBots();
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
                playerViewModel.Hand = await _handService.GetPlayerHand(player.Id);

                playerViewModelList.Add(playerViewModel);
            }

            return playerViewModelList;
        }

        public async Task SetPlayerToGame(string playerName)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var player = await _playerRepository.GetByName(playerName);
                var bots = await _playerRepository.GetBots();

                await _playerInGameRepository.RemoveAll();

                foreach (var bot in bots)
                {
                    await _playerInGameRepository.AddPlayer(bot.Id);
                    logger.Info(StringHelper.BotJoinGame(bot.Id));
                }

                await _playerInGameRepository.AddHuman(player.Id);
                logger.Info(StringHelper.HumanJoinGame(player.Id));
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<int>> GetPlayersIdInGame()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var playerIdList = await _playerInGameRepository.GetAll();

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

        public async Task PlaceBet(int playerId, int betValue)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var player = await _playerRepository.GetById(playerId);

                if (player.Points < betValue)
                {
                    throw new Exception(StringHelper.NotEnoughPoints(playerId, betValue));
                }

                if(betValue <= 0)
                {
                    throw new Exception(StringHelper.NoBetValue());
                }

                await _playerInGameRepository.PlaceBet(playerId, betValue);

                logger.Info(StringHelper.PlayerPlaceBet(playerId, betValue));
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<PlayerViewModel> GetHumanInGame()
        {
            try
            {
                var humanId = -1;
                humanId = await _playerInGameRepository.GetHuman();

                if (humanId == -1)
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

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
                playerViewModel.Hand = await _handService.GetPlayerHand(player.Id);

                return playerViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<DealerViewModel> GetDealer()
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
                dealerViewModel.Hand = await _handService.GetPlayerHand(dealer.Id);

                return dealerViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
