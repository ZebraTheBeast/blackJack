using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using Dapper.Contrib.Extensions;
using BlackJack.Entities.Properties;
using System.Linq;

namespace BlackJack.DataAccess.Repositories
{
	public class HandRepository : IHandRepository
	{
		private string _connectionString;

		public HandRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<int>> GetCardIdList(int playerId, int gameId)
		{
			var cards = new List<int>();
			var sqlQuery = "SELECT CardId FROM Hand WHERE PlayerId = @playerId AND GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				cards = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).ToList();
			}

			return cards;
		}

		public async Task<List<int>> GetCardIdListByGameId(int gameId)
		{
			var cards = new List<int>();
			var sqlQuery = "SELECT CardId FROM Hand WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				cards = (await db.QueryAsync<int>(sqlQuery, new { gameId })).ToList();
			}

			return cards;
		}

		public async Task GiveCardToPlayer(int playerId, int cardId, int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.InsertAsync(new Hand { CardId = cardId, GameId = gameId, PlayerId = playerId });
			}
		}

		public async Task RemoveAll(int gameId)
		{
			var sqlQuery = $"DELETE FROM Hand WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { gameId });
			}
		}
	}
}
