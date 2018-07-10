using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class InGame
    {
        public List<Player> Players { get; set; }
        public List<Card> Deck { get; set; }

        public InGame()
        {
            Players = new List<Player>();
            Deck = new List<Card>();
        }
    }
}
