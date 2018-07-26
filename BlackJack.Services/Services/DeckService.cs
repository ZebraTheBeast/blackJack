using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.Entity.Enum;
using BlackJack.Entity;
using BlackJack.DAL.Interface;
using BlackJack.DAL.Repository;
using BlackJack.BLL.Interface;
using BlackJack.Logger;

namespace BlackJack.BLL.Services
{
    public class DeckService
    {

        ICardRepository _cardRepository;
        IHandRepository _handRepository;

        public DeckService(ICardRepository cardRepository, IHandRepository handRepository)
        {

            _cardRepository = cardRepository;
            _handRepository = handRepository;
        }

        public List<int> RefreshAndShuffleDeck()
        {
            Random rng = new Random();
            var deck = new List<Card>();
            var cardIdList = new List<int>();
            deck = GetFullDeck();

            int n = deck.Count;

            for (var i = 0; i < n; i++)
            {
                int k = rng.Next(n);
                var value = deck[i];
                deck[i] = deck[k];
                deck[k] = value;
            }

            foreach (var card in deck)
            {
                cardIdList.Add(card.Id);
            }
            Logger.Logger.Info($"Deck war refreshed and shuffled.");

            return cardIdList;
        }

        public async Task GiveCardFromDeck(int playerId, int cardId)
        {
            await _handRepository.GiveCardToPlayer(playerId, cardId);
        }


        private List<Card> GetFullDeck()
        {
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

            FillDeckWithCard(deck, titleList, valueList);

            return deck;
        }

        private void FillDeckWithCard(List<Card> deck, List<string> cardNames, List<int> cardValues)
        {
            int cardColorValue = 0;
            int cardTitleValue = 0;
            int cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;

            for (int i = 0; i < Constant.DeckSize; i++)
            {
                var card = new Card();

                card.Id = i + 1;
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
    }
}
