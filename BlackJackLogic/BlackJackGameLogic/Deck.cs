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
            _fullDeck = GetNewDeck();
        }

        // TODO - подумать как избавиться от hand, нужен ли он
        // скорее всего отправить его в gameplay, но это не точно.
        public void GiveCard(Player player, DeckEntity deck)
        {
            var hand = new Hand();

            player.Hand.HandCard.Add(deck.CardList[0]);
            deck.CardList.Remove(deck.CardList[0]);
            hand.CountHandValue(player);
        }

        public void ShuffleDeck(DeckEntity deck)
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

        public DeckEntity GetNewDeck()
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
