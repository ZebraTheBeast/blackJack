using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;

namespace BlackJackLogic
{
    public class Gameplay
    {
        //TODO - minus solid, zhostko zavisit ot Deck, DeckEntity
        public Deck _deck;
        public DeckEntity _playingDeck = new DeckEntity();
        public List<Player> _players = new List<Player>();

        public Gameplay()
        {
            _deck = new Deck();
        }

        public void PlayerAdd(Player player)
        {
            _players.Add(player);
        }
        
        public void Dealing()
        {
            _deck.ShuffleDeck(_playingDeck);

            foreach (Player player in _players)
            {
                _deck.GiveCard(player, _playingDeck);
                _deck.GiveCard(player, _playingDeck);
            }
        }
       
        public void Turn(Player player)
        {
            ConsoleKeyInfo consoleKeyInfo;
            bool continueTurn = true;
            do
            {
                consoleKeyInfo = Console.ReadKey();
                if(consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    _deck.GiveCard(player, _playingDeck);
                }
                if (consoleKeyInfo.Key == ConsoleKey.Escape)
                {
                    continueTurn = false;
                }
                if(player.Hand.HandCardValue > 21)
                {
                    continueTurn = false;
                }

            } while (continueTurn);
        }

    }
}
