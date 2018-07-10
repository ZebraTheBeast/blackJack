using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Entity
{
    public class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public Hand Hand { get; set; }
    }
}
