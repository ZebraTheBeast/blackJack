using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BLL.Interface
{
    public interface IDeckService
    {
        List<int> GetNewRefreshedDeck();
        Task GiveCardFromDeck(int playerId, int cardId);
    }
}
