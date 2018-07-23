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
    public static class PointCheckerService
    {

        public static GameModel CheckPlayerWithDealer(PlayerModel player, PlayerModel dealer, GameModel gameModel)
        {
            if ((player.Hand.CardListValue > dealer.Hand.CardListValue) && (CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue)) && (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(dealer, Constant.WinValue))))
            {
                StringService.PlayerWon(gameModel, player.Name);
                gameModel = PointService.WinPoints(gameModel, player.Id);
                return gameModel;
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListIsBlackJack(dealer)) && (!CombinationCheckerService.PlayerHandCardListIsBlackJack(player))))
            {
                StringService.PlayerLose(gameModel, player.Name);
                gameModel = PointService.LosePoints(gameModel, player.Id);
                return gameModel;
            }

            if (player.Hand.CardListValue == dealer.Hand.CardListValue)
            {
                StringService.PlayerDraw(gameModel, player.Name);
                gameModel = PointService.AnnulPoints(gameModel, player.Id);
                return gameModel;
            }

            return gameModel;
        }

    }
}
