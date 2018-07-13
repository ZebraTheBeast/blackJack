using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.DAL.Interface;

namespace BlackJack.BLL.Services
{
    public class PointCheckerService
    {
        PointService _point;
        
        public PointCheckerService(IUnitOfWork unitOfWork)
        {
            _point = new PointService(unitOfWork);
        }

        public void CheckPlayerWithDealer(PlayerModel player, PlayerModel dealer)
        {
            if ((player.Hand.CardListValue > dealer.Hand.CardListValue) && (CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue)) && (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(dealer, Constant.WinValue))))
            {
                _point.WinPoints(player);
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListIsBlackJack(dealer)) && (!CombinationCheckerService.PlayerHandCardListIsBlackJack(player))))
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
