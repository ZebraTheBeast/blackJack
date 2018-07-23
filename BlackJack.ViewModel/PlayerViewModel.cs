using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class PlayerViewModel
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public HandViewModel Hand { get; set; }
    }
}
