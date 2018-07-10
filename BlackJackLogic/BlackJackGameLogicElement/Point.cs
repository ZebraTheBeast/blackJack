using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;

namespace BlackJackLogic.BlackJackGameLogicElement
{
    public static class Point
    {
        public static bool PutPoints(PlayerEntity player, int pointsValue)
        {
            if (player.Points >= pointsValue)
            {
                player.Hand.HandPoints = pointsValue;
                return true;
            }

            return false;
        }

        public static void LosePoints(PlayerEntity player)
        {
            player.Points -= player.Hand.HandPoints;
            AnnulPoints(player);
        }

        public static void WinPoints(PlayerEntity player)
        {
            player.Points += player.Hand.HandPoints;
            AnnulPoints(player);
        }

        public static void AnnulPoints(PlayerEntity player)
        {
            player.Hand.HandPoints = 0;
        }
    }
}
