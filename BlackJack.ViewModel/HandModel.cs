using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class HandModel
    {
        public List<CardModel> CardList { get; set; }
        public int CardListValue { get; set; }
        public int Points { get; set; }

        public HandModel()
        {
            CardList = new List<CardModel>();
        }
    }
}
