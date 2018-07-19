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
                gameModel.GameStat.Add($"Player {player.Name} has won.");
                gameModel = PointService.WinPoints(gameModel, player.Id);
            }

            if ((player.Hand.CardListValue < dealer.Hand.CardListValue)
                || (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(player, Constant.WinValue))
                || ((CombinationCheckerService.PlayerHandCardListIsBlackJack(dealer)) && (!CombinationCheckerService.PlayerHandCardListIsBlackJack(player))))
            {
                gameModel.GameStat.Add($"Player {player.Name} has lost.");
                gameModel = PointService.LosePoints(gameModel, player.Id);
            }

            if (player.Hand.CardListValue == dealer.Hand.CardListValue)
            {
                gameModel.GameStat.Add($"Player {player.Name} has draw with Dealer.");
                gameModel = PointService.AnnulPoints(gameModel, player.Id);
            }

            return gameModel;
        }
    }
}
