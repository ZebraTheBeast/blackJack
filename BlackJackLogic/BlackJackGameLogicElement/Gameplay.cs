using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;

namespace BlackJackLogic.BlackJackGameLogicElement
{
    public class Gameplay
    {     
        public DeckEntity _playingDeck;
        public List<PlayerEntity> _players;

        public Gameplay()
        {
            _playingDeck = new DeckEntity();
            _players = new List<PlayerEntity>();
        }

        public void PlayerAdd(PlayerEntity player)
        {
            _players.Add(player);
        }
        
        public void Dealing()
        {
            Deck.ShuffleDeck(_playingDeck);

            foreach (PlayerEntity player in _players)
            {
                Deck.GiveCard(player, _playingDeck);
                Deck.GiveCard(player, _playingDeck);
            }
        }
    }
}
