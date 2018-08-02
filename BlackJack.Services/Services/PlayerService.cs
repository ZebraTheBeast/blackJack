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

        public PlayerService(IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository)
        {
            _playerRepository = playerRepository;
            _playerInGameRepository = playerInGameRepository;
        }

        public async Task<List<PlayerViewModel>> GetBotsInGame()
        {
            var playerIdList = await _playerInGameRepository.GetBots();
            var playerViewModelList = new List<PlayerViewModel>();
            foreach (var playerId in playerIdList)
            {
                var player = await _playerRepository.GetById(playerId);

                var playerViewModel = new PlayerViewModel
                {
                    Id = player.Id,
                    Name = player.Name,
                    Points = player.Points,
                    Hand = new HandViewModel { CardList = new List<CardViewModel>() }
                };

                playerViewModelList.Add(playerViewModel);
            }

            return playerViewModelList;
        }

        public async Task SetPlayerToGame(string playerName)
        {
            var logger =  NLog.LogManager.GetCurrentClassLogger();

            await  _playerInGameRepository.RemoveAll();
            var player = await _playerRepository.GetByName(playerName);
            var bots = await _playerRepository.GetBots();
            foreach (var bot in bots)
            {
                await  _playerInGameRepository.AddPlayer(bot.Id);
                logger.Info(StringHelper.BotJoinGame(bot.Id));
            }
           
            await _playerInGameRepository.AddHuman(player.Id);
            logger.Info(StringHelper.HumanJoinGame(player.Id));
        }

        public async Task<IEnumerable<int>> GetPlayersIdInGame()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var playerIdList = await _playerInGameRepository.GetAll();

                if(playerIdList.Count() == 0)
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

        public async Task MakeBet(int playerId, int betValue)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            { 
                var player = await _playerRepository.GetById(playerId);
                if (player.Points < betValue)
                {
                    throw new Exception(StringHelper.NotEnoughPoints(playerId, betValue));
                }
                await _playerInGameRepository.MakeBet(playerId, betValue);
                logger.Info(StringHelper.PlayerMakeBet(playerId, betValue));
            }
            catch(Exception exception)
            {
                logger.Error(exception.Message);
                throw new Exception(StringHelper.NotEnoughPoints(betValue));
            }
        }

        public async Task<PlayerViewModel> GetHumanInGame()
        {
            try
            {
                var humanId = 0;
                humanId = await _playerInGameRepository.GetHuman();

                if(humanId == 0)
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                var player = await _playerRepository.GetById(humanId);

                var playerViewModel = new PlayerViewModel
                {
                    Id = player.Id,
                    Name = player.Name,
                    Points = player.Points,
                    Hand = new HandViewModel { CardList = new List<CardViewModel>() }
                };

                return playerViewModel;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        public async Task<DealerViewModel> GetDealer()
        {
            var dealer = await _playerRepository.GetByName("Dealer");

            var dealerViewModel = new DealerViewModel
            {
                Id = dealer.Id,
                Name = dealer.Name,
                Hand = new HandViewModel { CardList = new List<CardViewModel>() }
            };

            return dealerViewModel;
        }
    }
}
