using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public class Deck : DeckEntity
    {
        private static Random rng = new Random();

        public Deck()
        {
            _fullDeck = FillDeck();
        }
        
        public void GiveCard(Player player)
        {
            player._hand._handCard.Add(_playingDeck[0]);
            _playingDeck.Remove(_playingDeck[0]);
            player._hand.CountCardsValue();
        }

        public void Shuffle()
        {
            _playingDeck = _fullDeck;
            int n = _playingDeck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardEntity value = _playingDeck[k];
                _playingDeck[k] = _playingDeck[n];
                _playingDeck[n] = value;
            }
        }

        public List<CardEntity> FillDeck()
        {
            var deck = new List<CardEntity>();
            int colorValue = 0;
            int colorSize = Enum.GetNames(typeof(Color)).Length - 1;
            int titleValue = 2;
            int titleSize = Enum.GetNames(typeof(Title)).Length;
            int maxCardValue = 11;
            int imageCardValue = 10;
            int deckSize = titleSize * 4;

            for (int i = 0; i < deckSize; i++)
            {
                deck.Add(new CardEntity());
            }

            foreach(var card in deck)
            {
                card._title = (Title)titleValue;
                if (titleValue > maxCardValue)
                {
                    card._value = titleValue;
                }
                else
                {
                    card._value = imageCardValue;
                }

                card._cardColor = (Color)colorValue++;

                if(colorValue > colorSize)
                {
                    colorValue = 0;
                    titleValue++;
                }
            }
            return deck;
        }    
     }
}
