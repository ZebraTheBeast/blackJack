using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.Entity.Enum;


namespace BlackJack.BLL.Services
{
    public static class DeckService
    {
        private static Random _rng = new Random();
        
        public static List<CardModel> GetShuffledDeck()
        {
            List<CardModel> deck = new List<CardModel>();
            deck = GetNewDeck();
            int n = deck.Count;

            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                CardModel value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
            return deck;
        }

        public static void FillDeckWithCard(List<CardModel> deck, int end, List<string> cardNames, List<int> cardValues)
        {
            int cardColorValue = 0;
            int cardTitleValue = 0;
            int cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;

            for (int i = 0; i < end; i++)
            {
                var card = new CardModel();

                card.Id = i;
                card.Title = cardNames[cardTitleValue];
                card.Value = cardValues[cardTitleValue];
                card.Color = (CardColor)cardColorValue++;

                deck.Add(card);

                if (cardColorValue > cardColorSize)
                {
                    cardColorValue = 0;
                    cardTitleValue++;
                }
            }
        }

        public static List<CardModel> GetNewDeck()
        {
            var deck = new List<CardModel>();
            var valueList = Enumerable.Range(Constant.NumberStartCard, Constant.CountNumberCard).ToList();
            var titleList = valueList.ConvertAll<string>(delegate (int i) { return i.ToString(); });

            foreach (var value in Enum.GetNames(typeof(CardTitle)))
            {
                titleList.Add(value);
            }

            foreach (var value in Enum.GetValues(typeof(CardTitle)))
            {
                valueList.Add((int)value);
            }

            FillDeckWithCard(deck, Constant.DeckSize, titleList, valueList);

            return deck;
        }
    }
}
