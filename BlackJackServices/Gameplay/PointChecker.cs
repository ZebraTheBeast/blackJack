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
            if ((player.Hand.CardListValue > dealer.Hand.CardListValue) && (CombinationChecker.IsLess(player, Constant.WinValue))
                || ((CombinationChecker.IsLess(player, Constant.WinValue)) && (!CombinationChecker.IsLess(dealer, Constant.WinValue))))
            {
                Point.WinPoints(player);
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationChecker.IsLess(player, Constant.WinValue))
                || ((CombinationChecker.IsBlackJack(dealer)) && (!CombinationChecker.IsBlackJack(player))))
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
