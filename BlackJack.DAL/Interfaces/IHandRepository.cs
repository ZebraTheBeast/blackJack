using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IHandRepository
    {
        Task GiveCardToPlayerInGame(int playerId, int cardId, int gameId);
        Task<List<int>> GetCardsIdByPlayerId(int playerId, int gameId);
        Task RemoveAllCardsInHand(int gameId);
		Task<List<int>> GetCardsIdByGameId(int gameId);
	}
}
