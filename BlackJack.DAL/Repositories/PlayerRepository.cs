using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.Entities;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Data.SqlClient;
using BlackJack.Configurations;
using BlackJack.Entities.Enums;

namespace BlackJack.DataAccess.Repositories
{
	public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
	{
		private string _connectionString;

		public PlayerRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<Player>> GetBots(int botsAmount)
		{
			var players = new List<Player>();
			var playersIdWithoutPoints = new List<int>();
			var sqlQuery = "SELECT TOP(@botsAmount) * FROM Player WHERE Type = @playerType";

			using (var db = new SqlConnection(_connectionString))
			{
				var bots = await db.QueryAsync<Player>(sqlQuery, new { botsAmount, playerType = PlayerType.Bot });
				players.AddRange(bots);
			}

			return players;
		}

		public async Task<Player> GetPlayerByName(string name)
		{
			var sqlQuery = "SELECT * FROM Player WHERE Name = @name AND Type = @playerType";

			using (var db = new SqlConnection(_connectionString))
			{
				var player = (await db.QueryAsync<Player>(sqlQuery, new { name, playerType = PlayerType.Human })).FirstOrDefault();
				return player;
			}

		}

		public async Task UpdatePlayersPoints(List<long> playerIds, int newPointsValue)
		{
			var sqlQuery = "UPDATE Player SET Points = @newPointsValue WHERE Id IN @playerIds";

			using (var db = new SqlConnection(_connectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { newPointsValue, playerIds });
			}
		}

		public async Task<Player> GetDealer()
		{
			var sqlQuery = "SELECT TOP(1) * FROM Player WHERE Type = @playerType";

			using (var db = new SqlConnection(_connectionString))
			{
				var player = (await db.QueryAsync<Player>(sqlQuery, new { playerType = PlayerType.Dealer })).FirstOrDefault();
				return player;
			}
		}		
	}
}
