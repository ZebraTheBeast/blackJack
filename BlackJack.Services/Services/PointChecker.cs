using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;

namespace BlackJack.BLL.Services
{
    public class PointChecker
    {
        public void CheckPlayerWithDealer(PlayerModel player, PlayerModel dealer)
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
