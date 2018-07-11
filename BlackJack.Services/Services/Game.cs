using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.Services.Gameplay
{
    public class Game
    {
        public InGame _inGame = new InGame();

        public void PlayerAdd(Player player)
        {
            _inGame.Players.Add(player);
        }

        public void Dealing()
        {
            Deck.GetShuffledDeck(_inGame.Deck);

            foreach (Player player in _inGame.Players)
            {
                Deck.GiveCard(player, _inGame);
                _inGame = Deck.GiveCard2(player, _inGame.Deck, _inGame.Players);
            }
        }
    }
}
