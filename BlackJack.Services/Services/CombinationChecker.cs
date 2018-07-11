using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;

namespace BlackJack.BLL.Services
{
    public static class CombinationChecker
    {
        public static bool PlayerHandCardListIsBlackJack(PlayerModel player)
        {
            if (player.Hand.CardList.Count() != Constant.NumberCardForBlackJack)
            {
                return false;
            }

            if(player.Hand.CardListValue == Constant.WinValue)
            {
                return true;
            }

            foreach (var card in player.Hand.CardList)
            {
                if (card.Title != Constant.NameCardForBlackJack)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool PlayerHandCardListValueLessThenPointsValue(PlayerModel player, int pointsValue)
        {
            if (player.Hand.CardListValue <= pointsValue)
            {
                return true;
            }

            return false;
        }

        public static bool PlayerHandCardListValueIsTwentyOne(PlayerModel player)
        {
            if (player.Hand.CardListValue == Constant.WinValue)
            {
                return true;
            }

            return false;
        }
    }
}
