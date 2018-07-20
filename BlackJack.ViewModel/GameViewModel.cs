using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class GameViewModel
    {
        public List<PlayerViewModel> Players { get; set; }
        public List<CardViewModel> Deck { get; set; }
        public List<string> GameStat { get; set; }
        public int ButtonPushed { get; set; }
    }
}
