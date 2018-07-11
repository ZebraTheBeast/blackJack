using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;

namespace BlackJack.Services.Gameplay
{
    public static class Hand
    {
        // TODO - eto
        public static int GetHandValue(Player player)
        {
            int cardValue = 0;

            foreach (Card card in player.Hand.CardList)
            {
                cardValue += card.Value;
            }

            foreach (Card card in player.Hand.CardList)
            {
                if ((card.Title.ToString() == Constant.NameCardForBlackJack) && (cardValue > Constant.WinValue))
                {
                    cardValue -= Constant.ImageCardValue;
                }
            }

            if (CombinationChecker.PlayerHandCardListIsBlackJack(player))
            {
                cardValue = Constant.WinValue;
            }

            return cardValue;
        }
    }
}
