using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.Entities;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using BlackJack.Configurations;

namespace BlackJack.DataAccess.Repositories
{
	public class PlayerRepository : IPlayerRepository
	{
		private string _connectionString;

		public PlayerRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task CreateNewPlayer(Player player)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.InsertAsync(new Player { Name = player.Name, CreationDate = player.CreationDate });
			}
		}

		public async Task<List<Player>> GetBots(string playerName, int botsAmount)
		{
			var players = new List<Player>();
			var playersIdWithoutPoints = new List<int>();
			var dealer = await GetPlayerByName(Constant.DealerName);
			var sqlQuery = "SELECT TOP(@botsAmount) * FROM Player WHERE Name <> @playerName AND Name <> @dealerName";
			players.Add(dealer);

			using (var db = new SqlConnection(_connectionString))
			{
				var bots = await db.QueryAsync<Player>(sqlQuery, new { botsAmount, playerName, dealerName = Constant.DealerName });
				players.AddRange(bots);
			}

			return players;
		}

		public async Task<Player> GetPlayerByName(string name)
		{
			var player = new Player();

			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT * FROM Player WHERE Name = @name";
				player = (await db.QueryAsync<Player>(sqlQuery, new { name })).FirstOrDefault();
			}

			return player;
		}

		public async Task UpdatePlayerPoints(int playerId, int newPointsValue)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = $"UPDATE Player SET Points = @newPointsValue WHERE Id = @playerId";
				await db.ExecuteAsync(sqlQuery, new { newPointsValue, playerId });
			}
		}

		public async Task RestorePlayerPoints(int playerId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "UPDATE Player SET Points = @defaultPointsValue WHERE Id = @playerId";
				await db.ExecuteAsync(sqlQuery, new { defaultPointsValue = Constant.DefaultPointsValue, playerId });
			}
		}

		public async Task<Player> GetPlayerById(int id)
		{
			var player = new Player();

			using (var db = new SqlConnection(_connectionString))
			{
				player = await db.GetAsync<Player>(id);
			}

			return player;
		}

		public async Task<List<Player>> GetPlayersByIds(List<int> idList)
		{
			var players = new List<Player>();
			var sqlQuery = "SELECT * FROM Player WHERE Id IN @idList";

			using (var db = new SqlConnection(_connectionString))
			{
				players = (await db.QueryAsync<Player>(sqlQuery, new { idList })).ToList();
			}

			return players;
		}

		public async Task RestorePlayersPoints(List<int> playersId)
		{

			var sqlQuery = "UPDATE Player SET Points = @defaultPointsValue WHERE Id on @playersId";

			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { defaultPointsValue = Constant.DefaultPointsValue, playersId });
			}
		}
	}
}
