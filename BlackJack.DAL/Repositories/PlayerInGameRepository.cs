using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Repositories
{
	public class PlayerInGameRepository : IPlayerInGameRepository
	{
		private string _connectionString;

		public PlayerInGameRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task AddPlayer(int playerId, int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.InsertAsync(new PlayerInGame() { PlayerId = playerId, Bet = 0, GameId = gameId });
			}
		}

		public async Task<IEnumerable<int>> GetBotsInGame(int gameId, int humanId, int dealerId)
		{
			IEnumerable<int> players = new List<int>();

			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE PlayerId <> @humanId AND PlayerId <> @dealerId AND GameId = @gameId";
				players = await db.QueryAsync<int>(sqlQuery, new { humanId, dealerId, gameId });
			}

			return players;
		}

		public async Task<IEnumerable<int>> GetAll(int gameId)
		{
			IEnumerable<int> players = new List<int>();

			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE GameId = @gameId";
				players = await db.QueryAsync<int>(sqlQuery, new { gameId });
			}
			return players;
		}

		public async Task RemoveAll(int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "DELETE FROM PlayerInGame WHERE GameId = @gameId";
				await db.ExecuteAsync(sqlQuery, new { gameId });
			}
		}

		public async Task PlaceBet(int playerId, int betValue, int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.UpdateAsync(new PlayerInGame() { PlayerId = playerId, Bet = betValue, GameId = gameId });
			}
		}

		public async Task AnnulBet(int playerId, int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.UpdateAsync(new PlayerInGame() { PlayerId = playerId, Bet = 0, GameId = gameId });
			}
		}

		public async Task<int> GetBetByPlayerId(int playerId, int gameId)
		{
			int betValue;
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT Bet FROM PlayerInGame WHERE PlayerId = @playerId AND GameId = @gameId";
				betValue = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
			}
			return betValue;
		}

		public async Task<bool> IsInGame(int playerId, int gameId)
		{
			var player = 0;

			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE PlayerId = @playerId AND GameId = @gameId";
				player = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
			}

			if (player == 0)
			{
				return false;
			}

			return true;
		}
	}
}
