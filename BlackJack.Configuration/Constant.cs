using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.Configuration.Constant
{
    public class Constant
    {
        public const int WinValue = 21;
        public const int AceCardValue = 11;
        public const int ImageCardValue = 10;
        public const int DeckSize = 52;
        public const int NumberStartCard = 2;
        public const int CountNumberCard = 9;
        public const int NumberCardForBlackJack = 2;
        public const string NameCardForBlackJack = "Ace";
        public const int DefaultPointsValue = 1000;
        public const int ValueToStopDraw = 16;
        public const int MinPointsValueToPlay = 10;
    }
}
