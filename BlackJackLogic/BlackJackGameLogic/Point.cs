using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;

namespace BlackJackLogic.BlackJackGameLogic
{
    public static class Point
    {
        public static void PutPoints(PlayerEntity player, int pointsValue)
        {
            if(player.Points >= pointsValue)
            {
                player.Hand.HandPoints = pointsValue;
            }

            if(player.Points < pointsValue)
            {
                // TODO - нельзя поставить поинтов больше, чем есть в наличии
            }
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
