using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;
using BlackJackServices.BlackJackConstant;

namespace BlackJackLogic.BlackJackGameLogicElement
{
    public static class CombinationChecker
    {
        public static bool IsBlackJack(PlayerEntity player)
        {
            if(player.Hand.HandCard.Count() != BlackJackConstant.NumberCardForBlackJack)
            {
                return false;
            }

            foreach(var card in player.Hand.HandCard)
            {
                if(card.Title != BlackJackConstant.NameCardForBlackJack)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsLess(PlayerEntity player, int pointsValue)
        {
            if(player.Hand.HandCardValue <= pointsValue)
            {
                return true;
            }

            return false;
        }

        public static bool IsTO(PlayerEntity player)
        {
            if(player.Hand.HandCardValue == BlackJackConstant.WinValue)
            {
                return true;
            }

            return false;
        }
    }
}
