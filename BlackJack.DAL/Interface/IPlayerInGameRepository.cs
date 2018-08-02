using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.DAL.Interface
{
    public interface IPlayerInGameRepository
    {
        Task AddPlayer(int playerId);
        Task AddHuman(int playerId);
        Task RemoveAll();
        Task<IEnumerable<int>> GetAll();
        Task<IEnumerable<int>> GetBots();
        Task<int> GetHuman();
        Task<int> GetBetByPlayerId(int playerId);
        Task MakeBet(int playerId, int bet);
        Task AnnulBet(int playerId);
        Task<bool> IsInGame(int playerId);
    }
}
