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
        public DeckEntity _playingDeck = new DeckEntity();
        public List<PlayerEntity> _players = new List<PlayerEntity>();

        public Gameplay()
        {
        
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
       
        public void Turn(PlayerEntity player)
        {
            ConsoleKeyInfo consoleKeyInfo;
            bool continueTurn = true;
            do
            {
                consoleKeyInfo = Console.ReadKey();
                if(consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    Deck.GiveCard(player, _playingDeck);
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
