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

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
        }

        public async Task<List<PlayerViewModel>> GetBotsInGame()
        {
            var playerIdList = await _playerInGameRepository.GetBots();
            var playerViewModelList = new List<PlayerViewModel>();
            foreach (var playerId in playerIdList)
            {
                var player = await _playerRepository.GetById(playerId);

                var playerViewModel = new PlayerViewModel();
                playerViewModel.Id = player.Id;
                playerViewModel.Name = player.Name;
                playerViewModel.Points = player.Points;
                playerViewModel.Hand = new HandViewModel { CardList = new List<CardViewModel>() };
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
            }
            return null;
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
            }
        }

        public async Task<PlayerViewModel> GetHumanInGame()
        {
            var humanId = await _playerInGameRepository.GetHuman();

            var player = await _playerRepository.GetById(humanId);

            var playerViewModel = new PlayerViewModel();
            playerViewModel.Id = player.Id;
            playerViewModel.Name = player.Name;
            playerViewModel.Points = player.Points;
            playerViewModel.Hand = new HandViewModel { CardList = new List<CardViewModel>() };

            return playerViewModel;
        }

        public async Task<DealerViewModel> GetDealer()
        {
            var dealerViewModel = new DealerViewModel();
            var dealer = await _playerRepository.GetByName("Dealer");
            dealerViewModel.Id = dealer.Id;
            dealerViewModel.Name = dealer.Name;
            dealerViewModel.Hand = new HandViewModel { CardList = new List<CardViewModel>() };
            return dealerViewModel;
        }
    }
}
