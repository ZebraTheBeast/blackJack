using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IHandRepository
    {
        Task GiveCardToPlayer(int playerId, int cardId, int gameId);
        Task<List<int>> GetCardIdListByPlayerId(int playerId, int gameId);
        Task RemoveAll(int gameId);
		Task<List<int>> GetCardIdListByGameId(int gameId);
	}
}
