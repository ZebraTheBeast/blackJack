using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Linq;
using System;

namespace BlackJack.DataAccess.Repositories
{
	public class HandRepository : IHandRepository
	{
		private string _connectionString;

		public HandRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<int>> GetCardIdListByPlayerId(int playerId, int gameId)
		{
			var cards = new List<int>();
			var sqlQuery = @"SELECT CardId FROM Hand 
				INNER JOIN PlayerInGame ON Hand.PlayerInGameId = PlayerInGame.Id
				WHERE PlayerInGame.PlayerId = @playerId 
				AND PlayerInGame.GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				cards = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).ToList();
			}

			return cards;
		}

		public async Task<List<int>> GetCardIdListByGameId(int gameId)
		{
			var cards = new List<int>();
			var sqlQuery = @"SELECT CardId FROM Hand 
				INNER JOIN PlayerInGame ON Hand.PlayerInGameId = PlayerInGame.Id 
				WHERE PlayerInGame.GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				cards = (await db.QueryAsync<int>(sqlQuery, new { gameId })).ToList();
			}

			return cards;
		}

		public async Task GiveCardToPlayerInGame(int playerId, int cardId, int gameId)
		{
			var sqlQuery = @"INSERT INTO Hand (CardId, PlayerInGameId, CreationDate) VALUES(@cardId,
				(SELECT Id FROM PlayerInGame WHERE GameId = @gameId AND PlayerId = @playerId), @date)";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { cardId,  gameId, playerId, date = DateTime.Now});
			}
		}

		public async Task RemoveAll(int gameId)
		{
			var sqlQuery = "DELETE Hand FROM Hand INNER JOIN PlayerInGame ON Hand.PlayerInGameId = PlayerInGame.Id WHERE PlayerInGame.GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { gameId });
			}
		}
	}
}
