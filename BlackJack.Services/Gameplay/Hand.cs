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
        public static void CountHandValue(Player player)
        {
            player.Hand.CardListValue = 0;

            foreach (Card card in player.Hand.CardList)
            {
                player.Hand.CardListValue += card.Value;
            }

            foreach (Card card in player.Hand.CardList)
            {
                if ((card.Title.ToString() == Constant.NameCardForBlackJack) && (player.Hand.CardListValue > Constant.WinValue))
                {
                    player.Hand.CardListValue -= Constant.ImageCardValue;
                }
            }

            if (CombinationChecker.PlayerHandCardListIsBlackJack(player))
            {
                player.Hand.CardListValue = Constant.WinValue;
            }
        }
    }
}
