using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Interface
{
    public interface IHandService
    {
        HandViewModel GetPlayerHand(int playerId);
        int GetPlayerHandValue(int playerId);
        void RemoveAllCardsInHand();
    }
}
