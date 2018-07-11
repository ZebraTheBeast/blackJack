using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;

namespace BlackJack.Services.Gameplay
{
    public static class CombinationChecker
    {
        public static bool PlayerHandCardListIsBlackJack(Player player)
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

        public static bool PlayerHandCardListValueLessThenPointsValue(Player player, int pointsValue)
        {
            if (player.Hand.CardListValue <= pointsValue)
            {
                return true;
            }

            return false;
        }

        public static bool PlayerHandCardListValueIsTwentyOne(Player player)
        {
            if (player.Hand.CardListValue == Constant.WinValue)
            {
                return true;
            }

            return false;
        }
    }
}
