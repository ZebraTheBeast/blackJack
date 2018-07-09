using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackEntity
{
    public class DeckEntity
    {
        public List<CardEntity> CardList { get; set; }

        public DeckEntity()
        {
            CardList = new List<CardEntity>();
        }
    }
}
