using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;

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

        public List<PlayerViewModel> GetBotsInGame()
        {
            var playerIdList = _playerInGameRepository.GetBots();
            var playerViewModelList = new List<PlayerViewModel>();
            foreach (var playerId in playerIdList)
            {
                var player = _playerRepository.GetById(playerId);

                var playerViewModel = new PlayerViewModel();
                playerViewModel.Id = player.Id;
                playerViewModel.Name = player.Name;
                playerViewModel.Points = player.Points;
                playerViewModel.Hand = new HandViewModel { CardList = new List<CardViewModel>() };
                playerViewModelList.Add(playerViewModel);
            }

            return playerViewModelList;
        }

        private void SetBotsToGame()
        {
            var bots = _playerRepository.GetBots();
            foreach (var bot in bots)
            {
                _playerInGameRepository.AddPlayer(bot.Id);
            }
        }

        public void SetPlayerToGame(string playerName)
        {
            _playerInGameRepository.RemoveAll();
            var player = _playerRepository.GetByName(playerName);
            SetBotsToGame();
            _playerInGameRepository.AddHuman(player.Id);
        }

        public void RemoveAllPlayers()
        {
            _playerInGameRepository.RemoveAll();
        }

        public List<int> GetPlayersIdInGame()
        {
            var playerIdList = _playerInGameRepository.GetAll();
            return playerIdList;
        }

        public void MakeBet(int playerId, int betValue)
        {
            _playerInGameRepository.MakeBet(playerId, betValue);
        }

        public PlayerViewModel GetHumanInGame()
        {
            var humanId = _playerInGameRepository.GetHuman();

            var player = _playerRepository.GetById(humanId);

            var playerViewModel = new PlayerViewModel();
            playerViewModel.Id = player.Id;
            playerViewModel.Name = player.Name;
            playerViewModel.Points = player.Points;
            playerViewModel.Hand = new HandViewModel { CardList = new List<CardViewModel>() };

            return playerViewModel;
        }

        public DealerViewModel GetDealer()
        {
            var dealerViewModel = new DealerViewModel();
            var dealer = _playerRepository.GetByName("Dealer");
            dealerViewModel.Id = dealer.Id;
            dealerViewModel.Name = dealer.Name;
            dealerViewModel.Hand = new HandViewModel { CardList = new List<CardViewModel>() };
            return dealerViewModel;
        }
    }
}
