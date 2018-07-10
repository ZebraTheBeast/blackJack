using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
using BlackJack.Configuration.Constant;

namespace BlackJack.Services.Gameplay
{
    public class PointChecker
    {
        public void CheckPlayerWithDealer(Player player, Player dealer)
        {
            if ((player.Hand.CardListValue > dealer.Hand.CardListValue) && (CombinationChecker.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationChecker.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue)) && (!CombinationChecker.PlayerHandCardListValueLessThenPointsValue(dealer, Constant.WinValue))))
            {
                Point.WinPoints(player);
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationChecker.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationChecker.PlayerHandCardListIsBlackJack(dealer)) && (!CombinationChecker.PlayerHandCardListIsBlackJack(player))))
            {
                Point.LosePoints(player);
            }

            if (player.Hand.CardListValue == dealer.Hand.CardListValue)
            {
                Point.AnnulPoints(player);
            }
        }
    }
}
