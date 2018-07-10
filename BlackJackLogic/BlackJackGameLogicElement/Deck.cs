using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackServices.BlackJackEnum;
using BlackJackServices.BlackJackConstant;
using BlackJackEntity;

namespace BlackJackLogic.BlackJackGameLogicElement
{
    public static class Deck
    {
        private static Random rng = new Random();

        public static void GiveCard(PlayerEntity player, DeckEntity deck)
        {
            player.Hand.HandCard.Add(deck.CardList[0]);
            deck.CardList.Remove(deck.CardList[0]);
            Hand.CountHandValue(player);
        }

        public static void ShuffleDeck(DeckEntity deck)
        {
            deck = GetNewDeck();
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

        public static void FillDeckWithCard(DeckEntity deck, int end, List<string> cardNames, List<int> cardValues)
        {
            int cardColorValue = 0;
            int cardTitleValue = 0;
            int cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;

            for (int i = 0; i < end; i++)
            {
                var card = new CardEntity();

                card.Title = cardNames[cardTitleValue];
                card.Value = cardValues[cardTitleValue];
                card.CardColor = (CardColor)cardColorValue++;

                deck.CardList.Add(card);

                if (cardColorValue > cardColorSize)
                {
                    cardColorValue = 0;
                    cardTitleValue++;
                }
            }
        }

        public static DeckEntity GetNewDeck()
        {
            var deck = new DeckEntity();
            var valueList = Enumerable.Range(BlackJackConstant.NumberStartCard, BlackJackConstant.CountNumberCard).ToList();
            var titleList = valueList.ConvertAll<string>(delegate (int i) { return i.ToString(); });

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
