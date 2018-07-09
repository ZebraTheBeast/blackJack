using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public abstract class Hand : HandEntity
    {
        public void CountCardsValue()
        {
            //переменные в отдельный слой
            int aceThreshold = 21;
            _handCardValue = 0;

            foreach (CardEntity card in _handCard)
            {
                _handCardValue += card._value;
            }

            foreach (CardEntity card in _handCard)
            {
                if ((card._title.ToString() == "Ace") && (_handCardValue > aceThreshold))
                {
                    _handCardValue -= 10;
                }
            }
        }
    }
}
