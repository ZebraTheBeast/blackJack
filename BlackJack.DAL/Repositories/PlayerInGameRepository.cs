using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Data.SqlClient;
using BlackJack.Entities;
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

		public async Task<List<long>> GetBotsIdByGameId(long gameId)
		{
			var sqlQuery = @"SELECT PlayerId FROM PlayerInGame 
				INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id 
				WHERE IsHuman = 0 AND Player.Type = @playerType AND GameId = @gameId";

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


		public async Task<int> GetBetByPlayerId(long playerId, long gameId)
		{
			var sqlQuery = @"SELECT BetValue FROM PlayerInGame 
				WHERE PlayerId = @playerId AND GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				var betValue = (await db.QueryAsync<int>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
				return betValue;
			}
		}

		public async Task UpdateBet(List<long> playersId, long gameId, int betValue)
		{
			var sqlQuery = @"UPDATE PlayerInGame SET BetValue = @betValue 
				WHERE GameId = @gameId AND PlayerId IN @playersId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { betValue , gameId, playersId });
			}
		}

        public async Task<long> GetIdByPlayerIdAndGameId(long playerId, long gameId)
        {
            var sqlQuery = @"SELECT PlayerInGame.Id FROM PlayerInGame
                INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id
                WHERE PlayerId = @playerId AND GameId = @gameId";
            using (var db = new SqlConnection(_connectionString))
            {
                var playerInGame = (await db.QueryAsync<long>(sqlQuery, new { playerId, gameId })).FirstOrDefault();
                return playerInGame;
            }

        }
    }
}
