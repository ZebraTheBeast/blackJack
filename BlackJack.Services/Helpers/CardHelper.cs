using System;
using System.Collections.Generic;
using System.Linq;
using BlackJack.ViewModels;
using BlackJack.Entities.Enums;
using BlackJack.Configurations;
using NLog;

namespace BlackJack.BusinessLogic.Helper
{
	public static class CardHelper
	{
		public static CardViewModel GetCardById(int cardId, List<CardViewModel> deck)
		{
			var card = new CardViewModel();
			
			card = deck.FirstOrDefault(c => c.Id == cardId);

			var cardViewModel = new CardViewModel
			{
				Id = card.Id,
				Title = card.Title,
				Color = card.Color,
				Value = card.Value
			};

			return cardViewModel;
		}

		public static List<CardViewModel> GetFullDeck()
		{
			var cardColorValue = 0;
			var cardTitleValue = 0;
			var cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;
			var deck = new List<CardViewModel>();
			var valueList = Enumerable.Range(Constant.NumberStartCard, Constant.AmountNumberCard).ToList();
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
				var card = new CardViewModel
				{
					Id = i + 1,
					Title = titleList[cardTitleValue],
					Value = valueList[cardTitleValue],
					Color = ((CardColor)cardColorValue++).ToString()
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

		public static List<int> GetNewRefreshedDeck()
		{
			var logger = LogManager.GetCurrentClassLogger();
			var cardViewModelList = new List<CardViewModel>();
			var deck = new List<int>();

			cardViewModelList = GetFullDeck();
			foreach (var card in cardViewModelList)
			{
				deck.Add(card.Id);
			}

			deck = ShuffleDeck(deck);
			logger.Info(StringHelper.DeckShuffled());
			return deck;
		}

		public static List<int> LoadDeck(IEnumerable<int> cardIdList)
		{
			var cardViewModelList = new List<CardViewModel>();
			var deck = new List<int>();

			cardViewModelList = GetFullDeck();

			foreach (var cardId in cardIdList)
			{
				cardViewModelList.RemoveAll(c => c.Id == cardId);
			}

			foreach (var card in cardViewModelList)
			{
				deck.Add(card.Id);
			}

			deck = ShuffleDeck(deck);

			return deck;
		}

		private static List<int> ShuffleDeck(List<int> deck)
		{
			Random rng = new Random();
			int deckSize = deck.Count();

			for (var i = 0; i < deckSize; i++)
			{
				int index = rng.Next(deckSize);
				var value = deck[i];
				deck[i] = deck[index];
				deck[index] = value;
			}

			return deck;
		}

	}
}
