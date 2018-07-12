using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.DAL.Interface;
using BlackJack.BLL.Helper;

namespace BlackJack.BLL.Services
{
    public class PointChecker
    {
        Point _point;
        
        public PointChecker(IUnitOfWork unitOfWork)
        {
            _point = new Point(unitOfWork);
        }

        public void CheckPlayerWithDealer(PlayerModel player, PlayerModel dealer)
        {
            if ((player.Hand.CardListValue > dealer.Hand.CardListValue) && (CombinationChecker.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationChecker.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue)) && (!CombinationChecker.PlayerHandCardListValueLessThenPointsValue(dealer, Constant.WinValue))))
            {
                _point.WinPoints(player);
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationChecker.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationChecker.PlayerHandCardListIsBlackJack(dealer)) && (!CombinationChecker.PlayerHandCardListIsBlackJack(player))))
            {
                _point.LosePoints(player);
            }

            if (player.Hand.CardListValue == dealer.Hand.CardListValue)
            {
                _point.AnnulPoints(player);
            }
        }
    }
}
