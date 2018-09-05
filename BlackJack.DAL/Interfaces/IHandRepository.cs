﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IHandRepository
    {
        Task GiveCardToPlayer(int playerId, int cardId, int gameId);
        Task<IEnumerable<int>> GetCardIdList(int playerId, int gameId);
        Task RemoveAll(int gameId);
		Task<IEnumerable<int>> GetCardIdListByGameId(int gameId);
	}
}