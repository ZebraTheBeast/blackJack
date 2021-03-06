﻿using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerInGameRepository : IBaseRepository<PlayerInGame>
    {
        Task RemoveAllPlayersFromGame(long gameId);
        Task<List<long>> GetAllPlayersIdByGameId(long gameId);
        Task<List<long>> GetBotsIdByGameId(long gameId);
        Task<int> GetBetByPlayerId(long playerId, long gameId);
		Task UpdateBet(List<long> playerId, long gameId, int betValue);
		Task<long> GetHumanIdByGameId(long gameId);
        Task<List<PlayerInGame>> GetPlayersInGameByPlayerIds(List<long> playerIds);
    }
}
