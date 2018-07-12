using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.BLL.Helper;

namespace BlackJack.BLL.Services
{
    public class Game
    {
        public InGame _inGame = new InGame();

        public Play _play;

        public Game(IUnitOfWork unitOfWork)
        {
            _play = new Play(unitOfWork);
        }

        public void PlayerAdd(PlayerModel player)
        {
            _inGame.Players.Add(player);
        }

        public void Dealing()
        {
            _inGame.Deck = Deck.GetShuffledDeck();

            foreach (PlayerModel player in _inGame.Players)
            {
                _play.TakeCard(player, _inGame.Deck);
            }
        }
    }
}
