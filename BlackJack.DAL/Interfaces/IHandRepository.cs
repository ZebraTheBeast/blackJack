using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IHandRepository : IGenericRepository<Hand>
	{
        Task GiveCardToPlayerInGame(long playerId, long cardId, long gameId);
        Task<List<long>> GetCardsIdByPlayerId(long playerId, long gameId);
        Task RemoveAllCardsInHand(long gameId);
		Task<List<long>> GetCardsIdByGameId(long gameId);
	}
}
