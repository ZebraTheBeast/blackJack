using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Entity;
using BlackJack.Entity.Enum;
using BlackJack.Configuration.Constant;


namespace BlackJack.BLL.Helper
{
    public static class CardHelper
    {
        public static CardViewModel GetCardById(int cardId)
        {
            var deck = new List<Card>();
            var card = new Card();
            var cardViewModel = new CardViewModel();

            deck = GetFullDeck();
            card = deck.FirstOrDefault(c => c.Id == cardId);

            cardViewModel.Id = card.Id;
            cardViewModel.Title = card.Title;
            cardViewModel.Color = card.Color.ToString();
            cardViewModel.Value = card.Value;

            return cardViewModel;
        }

        public static List<Card> GetFullDeck()
        {
            var cardColorValue = 0;
            var cardTitleValue = 0;
            var cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;
            var deck = new List<Card>();
            var valueList = Enumerable.Range(Constant.NumberStartCard, Constant.CountNumberCard).ToList();
            var titleList = valueList.ConvertAll<string>(delegate (int i)
            {
                return i.ToString();
            });

            foreach (var value in Enum.GetNames(typeof(CardTitle)))
            {
                titleList.Add(value);
            }

            for (var i = 0; i < Enum.GetValues(typeof(CardTitle)).Length - 1; i++)
            {
                valueList.Add(Constant.ImageCardValue);
            }

            valueList.Add(Constant.AceCardValue);

            for (var i = 0; i < Constant.DeckSize; i++)
            {
                var card = new Card();

                card.Id = i + 1;
                card.Title = titleList[cardTitleValue];
                card.Value = valueList[cardTitleValue];
                card.Color = (CardColor)cardColorValue++;

                deck.Add(card);

                if (cardColorValue > cardColorSize)
                {
                    cardColorValue = 0;
                    cardTitleValue++;
                }
            }

            return deck;
        }
    }
}
