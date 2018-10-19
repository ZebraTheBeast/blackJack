using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Linq;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Repositories
{
    public class CardInHandRepository : GenericRepository<CardInHand>, ICardInHandRepository
	{
		private string _connectionString;

		public CardInHandRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<long>> GetCardsIdByPlayerIdAndGameId(long playerId, long gameId)
		{
			var sqlQuery = @"SELECT CardId FROM CardInHand 
				WHERE PlayerId = @playerId 
				AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				var cards = (await db.QueryAsync<long>(sqlQuery, new { playerId, gameId })).ToList();
				return cards;
			}
		}

		public async Task<List<long>> GetCardsIdByGameId(long gameId)
		{
			var sqlQuery = @"SELECT CardId FROM CardInHand 
				WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				var cards = (await db.QueryAsync<long>(sqlQuery, new { gameId })).ToList();
				return cards;
			}
		}

		public async Task RemoveAllCardsByGameId(long gameId)
		{
			var sqlQuery = "DELETE FROM CardInHand WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { gameId });
			}
		}
	}
}
