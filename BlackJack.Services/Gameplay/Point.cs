using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.Services.Gameplay
{
    public static class Point
    {
        public static bool PutPoints(Player player, int pointsValue)
        {
            if (player.Points >= pointsValue)
            {
                player.Hand.CardListValue = pointsValue;
                return true;
            }

            return false;
        }

        public static void LosePoints(Player player)
        {
            player.Points -= player.Hand.CardListValue;
            AnnulPoints(player);
        }

        public static void WinPoints(Player player)
        {
            player.Points += player.Hand.CardListValue;
            AnnulPoints(player);
        }

        public static void AnnulPoints(Player player)
        {
            player.Hand.CardListValue = 0;
        }
    }
}
