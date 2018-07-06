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
