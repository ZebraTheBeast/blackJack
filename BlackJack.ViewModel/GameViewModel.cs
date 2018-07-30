using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class GameViewModel
    {
        public DealerViewModel Dealer { get; set; }
        public PlayerViewModel Human { get; set; }
        public List<PlayerViewModel> Bots { get; set; }
        public List<int> Deck { get; set; }
        public string Options { get; set; }
    }
}
