using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface ICardInHandRepository : IBaseRepository<CardInHand>
	{
        Task<List<long>> GetCardsIdByPlayerIdAndGameId(long playerId, long gameId);
        Task RemoveAllCardsByGameId(long gameId);
		Task<List<long>> GetCardsIdByGameId(long gameId);
	}
}
