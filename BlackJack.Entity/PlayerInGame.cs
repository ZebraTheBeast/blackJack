using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Entity
{
    public class PlayerInGame
    {
        public int PlayerId { get; set; }
        public int Bet { get; set; }
		public bool Humanity { get; set; }
		public int GameId { get; set; }
    }
}
