using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerInGameRepository
    {
        Task AddPlayer(int playerId, int gameId);
        Task RemoveAll(int gameId);
        Task<IEnumerable<int>> GetAll(int gameId);
        Task<IEnumerable<int>> GetBotsInGame(int gameId, int humanId, int dealerId);
        Task<int> GetBetByPlayerId(int playerId, int gameId);
        Task PlaceBet(int playerId, int bet, int gameId);
        Task AnnulBet(int playerId, int gameId);
        Task<bool> IsInGame(int playerId, int gameId);
    }
}
