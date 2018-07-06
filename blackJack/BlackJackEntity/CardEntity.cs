using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public enum Color
    {
        diamonds, hearts, spades, clubs
    }

    public enum Title
    {
       two = 2, three = 3, four = 4, five = 5, six = 6, seven = 7, eight = 8, nine = 9, ten = 10, Jack = 12, Queen = 13, King = 14, Ace = 11
    }
    public class CardEntity
    {
        public Title _title { get; set; }
        public int _value { get; set; }
        public Color _cardColor { get; set; }
    }
}
