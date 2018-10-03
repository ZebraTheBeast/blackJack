using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;
using BlackJack.Configurations;
using BlackJack.Entities.Enums;

namespace BlackJack.DataAccess.Repositories
{
	public class PlayerInGameRepository : GenericRepository<PlayerInGame>, IPlayerInGameRepository
	{
		private string _connectionString;

		public PlayerInGameRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task Add(long playerId, long gameId, bool isHuman)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.InsertAsync(new PlayerInGame() { PlayerId = playerId, BetValue = 0, GameId = gameId, IsHuman = isHuman });
			}
		}

		public async Task<List<long>> GetBotsIdByGameId(long gameId)
		{
			var sqlQuery = "SELECT PlayerId FROM PlayerInGame INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id WHERE IsHuman = 0 AND Player.Type = @playerType AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				var players = (await db.QueryAsync<long>(sqlQuery, new { playerType = PlayerType.Bot, gameId })).ToList();
				return players;
			}
		}

		public async Task<long> GetHumanIdByGameId(long gameId)
		{
			var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE GameId = @gameId AND IsHuman = 1";
			using (var db = new SqlConnection(_connectionString))
			{
				long humanId = (await db.QueryAsync<long>(sqlQuery, new { gameId })).FirstOrDefault();
				return humanId;
			}
		}

		public async Task<List<long>> GetAllPlayersIdByGameId(long gameId)
		{
			var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				var playersId = (await db.QueryAsync<long>(sqlQuery, new { gameId })).ToList();
				return playersId;
			}
		}

		public async Task RemoveAllPlayersFromGame(long gameId)
		{
			var sqlQuery = "DELETE FROM PlayerInGame WHERE GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { gameId });
			}
		}

		public async Task PlaceBet(long playerId, int betValue, long gameId)
		{
			var sqlQuery = "UPDATE PlayerInGame SET BetValue = @betValue WHERE GameId = @gameId AND PlayerId = @playerId";

			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { betValue, gameId, playerId });
			}
		}

		public async Task AnnulBet(long playerId, long gameId)
		{
			var sqlQuery = "UPDATE PlayerInGame SET BetValue = 0 WHERE GameId = @gameId AND PlayerId = @playerId";

			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { gameId, playerId });
			}
		}

		public async Task AnnulBet(List<long> playersId, long gameId)
		{
			var sqlQuery = "UPDATE PlayerInGame SET BetValue = 0 WHERE GameId = @gameId AND PlayerId in @playersId";

			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { gameId, playersId });
			}
		}

		public async Task<int> GetBetByPlayerId(long playerId, long gameId)
		{
			var sqlQuery = "SELECT BetValue FROM PlayerInGame WHERE PlayerId = @playerId AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				var betValue = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
				return betValue;
			}
		}

		public async Task<long> IsInGame(long playerId, long gameId)
		{
			var sqlQuery = "SELECT PlayerId FROM PlayerInGame WHERE PlayerId = @playerId AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				var player = (await db.QueryAsync<long>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
				return player;
			}
		}

		public async Task PlaceBet(List<long> playersId, long gameId)
		{
			var sqlQuery = "UPDATE PlayerInGame SET BetValue = @betValue WHERE GameId = @gameId AND PlayerId IN @playersId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { betValue = Constant.BotsBetValue, gameId, playersId });
			}
		}
	}
}
