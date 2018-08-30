using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using BlackJack.DataAccess.Interfaces;

namespace BlackJack.DataAccess.Repositories
{
	public class HandRepository : IHandRepository
    {
		private string _connectionString;

		public HandRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

        public async Task<IEnumerable<int>> GetCardIdList(int playerId, int gameId)
        {
            IEnumerable<int> cards = new List<int>();

            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"SELECT CardId FROM Hand WHERE PlayerId = {playerId} AND GameId = {gameId}";
                cards = await db.QueryAsync<int>(sqlQuery);
            }

            return cards;
        }

		public async Task<IEnumerable<int>> GetCardIdListByGameId(int gameId)
		{
			IEnumerable<int> cards = new List<int>();

			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = $"SELECT CardId FROM Hand WHERE GameId = {gameId}";
				cards = await db.QueryAsync<int>(sqlQuery);
			}

			return cards;
		}

        public async Task GiveCardToPlayer(int playerId, int cardId, int gameId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"INSERT INTO Hand (PlayerId, CardId, GameId) VALUES({playerId}, {cardId}, {gameId})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task RemoveAll(int gameId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"DELETE FROM Hand WHERE GameId = {gameId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }
    }
}
