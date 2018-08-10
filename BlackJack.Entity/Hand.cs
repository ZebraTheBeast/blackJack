using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Entity.Properties
{
    public class Hand
    {
        public int PlayerId { get; set; }
        public int CardId { get; set; }
		public int BetValue { get; set; }
    }
}
