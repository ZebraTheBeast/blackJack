﻿using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerInGameRepository
    {
        Task AddPlayer(int playerId, int gameId);
        Task RemoveAll(int gameId);
        Task<List<int>> GetAll(int gameId);
        Task<List<int>> GetBotsInGame(int gameId, int humanId, int dealerId);
        Task<int> GetBetByPlayerId(int playerId, int gameId);
        Task PlaceBet(int playerId, int bet, int gameId);
		Task PlaceBet(List<int> playerId, int gameId);
		Task AnnulBet(int playerId, int gameId);
		Task AnnulBet(List<int> playersId, int gameId);
		Task<int> IsInGame(int playerId, int gameId);
		Task<List<PlayerInGame>> GetPlayersInGame(List<int> playersId, int gameId);
    }
}
