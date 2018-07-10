using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.Services.Gameplay
{
    public class Gameplay
    {
        public List<Card> _playingDeck = new List<Card>();
        public List<Player> _players = new List<Player>();

        public void PlayerAdd(Player player)
        {
            _players.Add(player);
        }

        public void Dealing()
        {
            Deck.ShuffleDeck(_playingDeck);

            foreach (Player player in _players)
            {
                Deck.GiveCard(player, _playingDeck);
                Deck.GiveCard(player, _playingDeck);
            }
        }
    }
}
