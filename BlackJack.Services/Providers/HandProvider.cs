using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Helper;
using BlackJack.ViewModels;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Configurations;
using NLog;

namespace BlackJack.BusinessLogic.Providers
{
	public class HandProvider : IHandProvider
	{
		IHandRepository _handRepository;

		public HandProvider(IHandRepository handRepository)
		{
			_handRepository = handRepository;
		}

		public async Task<HandViewModel> GetPlayerHand(int playerId, int gameId)
		{
			var deck = CardHelper.GetFullDeck();
			var hand = new HandViewModel
			{
				CardList = new List<CardViewModel>()
			};

			var cardsIdList = await _handRepository.GetCardIdList(playerId, gameId);

			foreach (var cardId in cardsIdList)
			{
				var card = CardHelper.GetCardById(cardId, deck);
				hand.CardList.Add(card);
			}

			hand.CardListValue = CountCardsValue(hand.CardList);

			return hand;
		}

		public async Task<int> GetPlayerHandValue(int playerId, int gameId)
		{
			var deck = CardHelper.GetFullDeck();
			var cards = new List<CardViewModel>();
			var playerCardsIdList = await _handRepository.GetCardIdList(playerId, gameId);

			foreach (var cardId in playerCardsIdList)
			{
				var card = CardHelper.GetCardById(cardId, deck);
				cards.Add(card);
			}

			var handValue = CountCardsValue(cards);

			return handValue;

		}

		private int CountCardsValue(List<CardViewModel> cards)
		{
			var cardListValue = 0;

			foreach (var card in cards)
			{
				cardListValue += card.Value;
			}

			foreach (var card in cards)
			{
				if ((card.Title == Constant.NameCardForBlackJack)
					&& (cardListValue > Constant.WinValue))
				{
					cardListValue -= Constant.ImageCardValue;
				}
			}

			if (cards.Count() != Constant.NumberCardForBlackJack)
			{
				return cardListValue;
			}

			foreach (var card in cards)
			{
				if (card.Title != Constant.NameCardForBlackJack)
				{
					return cardListValue;
				}
			}

			cardListValue = Constant.WinValue;
			return cardListValue;
		}

		public async Task RemoveAllCardsInHand(int gameId)
		{
			await _handRepository.RemoveAll(gameId);
		}

		public async Task GiveCardFromDeck(int playerId, int cardId, int gameId)
		{
			var logger = LogManager.GetCurrentClassLogger();
			await _handRepository.GiveCardToPlayer(playerId, cardId, gameId);
			logger.Info(StringHelper.PlayerDrawCard(playerId, cardId, gameId));
		}

		public async Task<IEnumerable<int>> GetCardsInGame(int gameId)
		{
			var cardsList = await _handRepository.GetCardIdListByGameId(gameId);
			return cardsList;
		}
	}
}
