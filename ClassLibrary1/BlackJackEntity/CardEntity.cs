using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.BlackJackEnum;

namespace BlackJackEntity
{
    public class CardEntity
    {
        public string Title { get; set; }
        public int Value { get; set; }
        public CardColor CardColor { get; set; }
    }
}
