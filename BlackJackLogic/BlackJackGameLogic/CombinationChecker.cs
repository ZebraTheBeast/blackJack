using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;
using Services.BlackJackConstant;

namespace BlackJackLogic.BlackJackGameLogic
{
    public static class CombinationChecker
    {
        public static bool IsBlackJack(PlayerEntity player)
        {
            if(player.Hand.HandCard.Count() != 2)
            {
                return false;
            }

            foreach(var card in player.Hand.HandCard)
            {
                if(card.Title != "Ace")
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
