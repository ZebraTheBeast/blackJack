using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity.Enum;

namespace BlackJack.Entity
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public CardColor Color { get; set; }
        public int Value { get; set; }
    }
}
