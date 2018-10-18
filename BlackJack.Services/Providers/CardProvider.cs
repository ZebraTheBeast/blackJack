using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using BlackJack.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Providers
{
	public class CardProvider : ICardProvider
	{
		ICardRepository _cardRepository;

		public CardProvider(ICardRepository cardRepository)
		{
			_cardRepository = cardRepository;
		}

		public async Task RestoreCardsInDb()
		{
			IEnumerable<Card> cardsInDb = await _cardRepository.GetAll();

			if (cardsInDb.Count() != Constant.DeckSize)
			{
				await _cardRepository.DeleteAll();
				List<Card> cards = GenerateCards();
				await _cardRepository.Add(cards);
			}
		}

		public async Task<List<long>> LoadInGameDeck(List<long> cardsInGame)
		{
			var deck = new List<long>();
			Random randomNumericGenerator = new Random();
			List<Card> cards = (await _cardRepository.GetAll()).ToList();

			foreach (var cardId in cardsInGame)
			{
				cards.RemoveAll(c => c.Id == cardId);
			}

			foreach (var card in cards)
			{
				deck.Add(card.Id);
			}

			for (var i = 0; i < deck.Count(); i++)
			{
				var index = randomNumericGenerator.Next(deck.Count());
				var value = deck[i];
				deck[i] = deck[index];
				deck[index] = value;
			}

			return deck;
		}

		public async Task<List<Card>> GetCardsByIds(List<long> idList)
		{
			List<Card> cards = await _cardRepository.GetCardsById(idList);
			return cards;
		}

		private List<Card> GenerateCards()
		{
			var cardColorValue = 0;
			var cardTitleValue = 0;
			var cardColorSize = Enum.GetNames(typeof(CardSuit)).Length - 1;
			var deck = new List<Card>();
			var valueList = Enumerable.Range(Constant.NumberStartCard, Constant.AmountNumberCard).ToList();
			List<string> titleList = valueList.ConvertAll<string>(delegate (int value)
			{
				return value.ToString();
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
				var card = new Card
				{
					Id = i + 1,
					Title = titleList[cardTitleValue],
					Value = valueList[cardTitleValue],
					Suit = (CardSuit)cardColorValue++
				};

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
