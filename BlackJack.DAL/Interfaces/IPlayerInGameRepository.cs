using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerInGameRepository : IGenericRepository<PlayerInGame>
    {
        Task AddPlayerToGame(long playerId, long gameId, bool isHuman);
        Task RemoveAllPlayersFromGame(long gameId);
        Task<List<long>> GetAllPlayersIdByGameId(long gameId);
        Task<List<long>> GetBotsIdByGameId(long gameId);
        Task<int> GetBetByPlayerId(long playerId, long gameId);
        Task PlaceBet(long playerId, int bet, long gameId);
		Task PlaceBet(List<long> playerId, long gameId);
		Task AnnulBet(long playerId, long gameId);
		Task AnnulBet(List<long> playersId, long gameId);
		Task<long> IsInGame(long playerId, long gameId);
		Task<long> GetHumanIdByGameId(long gameId);
	}
}
