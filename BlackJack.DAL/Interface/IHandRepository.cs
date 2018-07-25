using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.DAL.Interface
{
    public interface IHandRepository
    {
        Task GiveCardToPlayer(int playerId, int cardId);
        Task<IEnumerable<int>> GetIdCardsByPlayerId(int playerId);
        Task RemoveAll();
    }
}
