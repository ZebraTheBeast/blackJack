using System.Collections.Generic;
using System.Linq;
using BlackJack.ViewModels;
using BlackJack.Configurations;

namespace BlackJack.BusinessLogic.Helper
{
	public static class CardHelper
	{
	
		public static int CountCardsValue(List<CardViewModel> cards)
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

	}
}
