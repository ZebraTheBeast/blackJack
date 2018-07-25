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
    public class PlayerService
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
            await  _playerInGameRepository.RemoveAll();
            var player = await _playerRepository.GetByName(playerName);
            var bots = await _playerRepository.GetBots();
            foreach (var bot in bots)
            {
                await  _playerInGameRepository.AddPlayer(bot.Id);
            }
           
            await _playerInGameRepository.AddHuman(player.Id);
        }

        public async Task<IEnumerable<int>> GetPlayersIdInGame()
        {
            var playerIdList = await _playerInGameRepository.GetAll();
            return playerIdList;
        }

        public async Task<bool> MakeBet(int playerId, int betValue)
        {
            var player = await _playerRepository.GetById(playerId);
            if(player.Points < betValue)
            {
                return false;
            }
            await _playerInGameRepository.MakeBet(playerId, betValue);
            return true;
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
