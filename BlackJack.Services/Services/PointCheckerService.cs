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

        public GameModel CheckPlayerWithDealer(PlayerModel player, PlayerModel dealer, GameModel gameModel)
        {
            if ((player.Hand.CardListValue > dealer.Hand.CardListValue) && (CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue)) && (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(dealer, Constant.WinValue))))
            {
                gameModel = PointService.WinPoints(gameModel, player.Id);
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListIsBlackJack(dealer)) && (!CombinationCheckerService.PlayerHandCardListIsBlackJack(player))))
            {
                gameModel = PointService.LosePoints(gameModel, player.Id);
            }

            if (player.Hand.CardListValue == dealer.Hand.CardListValue)
            {
                gameModel = PointService.AnnulPoints(gameModel, player.Id);
            }

            return gameModel;
        }
    }
}
