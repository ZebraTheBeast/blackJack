using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.DAL.Interface
{
    public interface IHandRepository
    {
        void GiveCardToPlayer(int playerId, int cardId);
        List<int> GetIdCardsByPlayerId(int playerId);
        void RemoveAll();
    }
}
