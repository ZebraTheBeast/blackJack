using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackEntity;
using Services.BlackJackConstant;

namespace BlackJackLogic
{
    public class Hand 
    {
        public void CountHandValue(Player player)
        {
            player.Hand.HandCardValue = 0;

            foreach (CardEntity card in player.Hand.HandCard)
            {
                player.Hand.HandCardValue += card.Value;
            }

            foreach (CardEntity card in player.Hand.HandCard)
            {
                if ((card.Title.ToString() == "Ace") && (player.Hand.HandCardValue > BlackJackConstant.AceThreshold))
                {
                    player.Hand.HandCardValue -= 10;
                }
            }
        }
    }
}
