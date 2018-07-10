using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;
using BlackJackServices.BlackJackConstant;

namespace BlackJackLogic.BlackJackGameLogicElement
{
    public class PointChecker
    {
        public void CheckPlayerWithDealer(PlayerEntity player, PlayerEntity dealer)
        {
            if ((player.Hand.HandCardValue > dealer.Hand.HandCardValue) && (CombinationChecker.IsLess(player, BlackJackConstant.WinValue))
                || ((CombinationChecker.IsLess(player, BlackJackConstant.WinValue)) && (!CombinationChecker.IsLess(dealer, BlackJackConstant.WinValue))))
            {
                Point.WinPoints(player);
            }

            if ((player.Hand.HandCardValue < dealer.Hand.HandCardValue)
                || (!CombinationChecker.IsLess(player, BlackJackConstant.WinValue))
                || ((CombinationChecker.IsBlackJack(dealer)) && (!CombinationChecker.IsBlackJack(player))))
            {
                Point.LosePoints(player);
            }

            if (player.Hand.HandCardValue == dealer.Hand.HandCardValue)
            {
                Point.AnnulPoints(player);
            }

        }
    }
}
