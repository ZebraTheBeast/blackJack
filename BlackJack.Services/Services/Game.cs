using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;

namespace BlackJack.BLL.Services
{
    public class Game : IGame
    {
        public GameModel _gameModel = new GameModel();

        public Play _play;

        public Game(IUnitOfWork unitOfWork)
        {
            _play = new Play(unitOfWork);
        }

        public void AddPlayer(PlayerModel player)
        {
            _gameModel.Players.Add(player);
        }

        public void Dealing()
        {
            _gameModel.Deck = Deck.GetShuffledDeck();

            foreach (PlayerModel player in _gameModel.Players)
            {
                _play.TakeCard(player, _gameModel.Deck);
                _play.TakeCard(player, _gameModel.Deck);
            }
        }

        public LoginPlayersModel Test()
        {
            LoginPlayersModel loginPlayerModel = new LoginPlayersModel();
            loginPlayerModel.PlayerList.Add(new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(), Points = 100 });
            return loginPlayerModel;
        }
    }
}
