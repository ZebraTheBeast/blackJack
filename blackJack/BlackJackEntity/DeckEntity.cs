using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public abstract class DeckEntity
    {
        public List<CardEntity> FullDeck { get; set; }
        public List<CardEntity> PlayingDeck { get; set; }
    }
}
