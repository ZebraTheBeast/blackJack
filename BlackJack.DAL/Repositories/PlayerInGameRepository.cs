using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;
using BlackJack.Configurations;

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

		public async Task<List<int>> GetBotsInGame(int gameId, int humanId, int dealerId)
		{
			var players = new List<int>();
			var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE PlayerId <> @humanId AND PlayerId <> @dealerId AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				players = (await db.QueryAsync<int>(sqlQuery, new { humanId, dealerId, gameId })).ToList();
			}

			return players;
		}

		public async Task<List<int>> GetAll(int gameId)
		{
			var players = new List<int>();
			var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				players = (await db.QueryAsync<int>(sqlQuery, new { gameId })).ToList();
			}
			return players;
		}

		public async Task RemoveAll(int gameId)
		{
			var sqlQuery = "DELETE FROM PlayerInGame WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
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

		public async Task AnnulBet(List<int> playersId, int gameId)
		{
			var sqlQuery = "UPDATE PlayerInGame SET Bet = 0 WHERE GameId = @gameId AND PlayerId in @playersId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { gameId, playersId });
			}
		}

		public async Task<int> GetBetByPlayerId(int playerId, int gameId)
		{
			int betValue;
			var sqlQuery = "SELECT Bet FROM PlayerInGame WHERE PlayerId = @playerId AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				betValue = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
			}
			return betValue;
		}

		public async Task<int> IsInGame(int playerId, int gameId)
		{
			var player = 0;
			var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE PlayerId = @playerId AND GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				player = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
			}
			return player;
		}

		public async Task PlaceBet(List<int> playersId, int gameId)
		{
			var sqlQuery = "UPDATE PlayerInGame SET Bet = @betValue WHERE GameId = @gameId AND PlayerId IN @playersId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { betValue = Constant.BotsBetValue, gameId, playersId });
			}
		}

		public async Task<List<PlayerInGame>> GetPlayersInGame(List<int> playersId, int gameId)
		{
			var players = new List<PlayerInGame>();
			var sqlQuery = "SELECT * FROM PlayerInGame WHERE GameId = @gameId AND PlayerId in @playersId";
			using (var db = new SqlConnection(_connectionString))
			{
				players = (await db.QueryAsync<PlayerInGame>(sqlQuery, new { gameId, playersId })).ToList();	
			}
			return players;
		}
	}
}
