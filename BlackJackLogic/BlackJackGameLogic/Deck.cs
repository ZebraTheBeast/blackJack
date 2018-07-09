using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.BlackJackEnum;
using Services.BlackJackConstant;
using BlackJackEntity;

namespace BlackJackLogic
{
    public class Deck
    {
        private static Random rng = new Random();
        private DeckEntity _fullDeck = new DeckEntity();

        public Deck()
        {
            _fullDeck = FillDeck();
        }

        public void GiveCard(Player player, DeckEntity deck)
        {
            var hand = new Hand();

            player.Hand.HandCard.Add(deck.CardList[0]);
            deck.CardList.Remove(deck.CardList[0]);
            hand.CountHandValue(player);
        }

        public void Shuffle(DeckEntity deck)
        {
            deck = _fullDeck;
            int n = deck.CardList.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardEntity value = deck.CardList[k];
                deck.CardList[k] = deck.CardList[n];
                deck.CardList[n] = value;
            }
        }

        public void FillDeckWithCard(DeckEntity deck, int end, List<string> cardNames, List<int> cardValues)
        {
            int cardColorValue = 0;
            int cardTitleValue = 0;
            int cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;

            for (int i = 0; i < end; i++)
            {
                deck.CardList[i].Title = cardNames[cardTitleValue];
                deck.CardList[i].Value = cardValues[cardTitleValue];
                deck.CardList[i].CardColor = (CardColor)cardColorValue++;

                if (cardColorValue > cardColorSize)
                {
                    cardColorValue = 0;
                    cardTitleValue++;
                }
            }
        }



        public DeckEntity FillDeck()
        {
            var deck = new DeckEntity();
            var valueList = Enumerable.Range(2, 9).ToList();
            var titleList = valueList.ConvertAll<string>(delegate (int i) { return i.ToString(); });

            for (int i = 0; i < BlackJackConstant.DeckSize; i++)
            {
                var card = new CardEntity();
                deck.CardList.Add(card);
            }

            foreach (var value in Enum.GetNames(typeof(CardTitle)))
            {
                titleList.Add(value);
            }

            foreach (var value in Enum.GetValues(typeof(CardTitle)))
            {
                valueList.Add((int)value);
            }

            FillDeckWithCard(deck, BlackJackConstant.DeckSize, titleList, valueList);

            return deck;
        }
    }
}
