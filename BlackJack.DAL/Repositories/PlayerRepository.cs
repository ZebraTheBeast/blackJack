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

		public async Task Create(Player player)
        {
            using (var db = new SqlConnection(_connectionString))
            {
				await db.InsertAsync(new Player { Name = player.Name, CreationDate = player.CreationDate});
			}
        }

        public async Task<IEnumerable<Player>> GetBots(string playerName, int botsAmount)
        {
			var players = new List<Player>();
			var dealer = await GetByName(Constant.DealerName);

			players.Add(dealer);

			using (var db = new SqlConnection(_connectionString))
            {
				var sqlQuery = "SELECT TOP(@botsAmount) * FROM Player WHERE Name <> @playerName AND Name <> @dealerName";
                var bots = await db.QueryAsync<Player>(sqlQuery, new { botsAmount, playerName, dealerName = Constant.DealerName });
				players.AddRange(bots);
			}

            foreach (var player in players)
            {
                player.Points = await PointsCheck(player.Id, player.Points);
            }

            return players;
        }

        public async Task<Player> GetByName(string name)
        {
            var player = new Player();

            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Player WHERE Name = @name";
                player = (await db.QueryAsync<Player>(sqlQuery, new { name })).FirstOrDefault();
            }

            if (player == null)
            {
                player = new Player { Name = name };
                await Create(player);
                return await GetByName(name);
            }

			player.Points = await PointsCheck(player.Id, player.Points);

			return player;
        }

        public async Task UpdatePoints(int playerId, int newPointsValue)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"UPDATE Player SET Points = @newPointsValue WHERE Id = @playerId";
                await db.ExecuteAsync(sqlQuery, new { newPointsValue, playerId });
            }
        }

        public async Task RestorePoints(int playerId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Player SET Points = @defaultPointsValue WHERE Id = @playerId";
                await db.ExecuteAsync(sqlQuery, new { defaultPointsValue = Constant.DefaultPointsValue , playerId });
            }
        }

        public async Task<Player> GetById(int id)
        {
            var player = new Player();

            using (var db = new SqlConnection(_connectionString))
            {
				player = await db.GetAsync<Player>(id);
				player.Points = await PointsCheck(player.Id, player.Points);
			}

            return player;
        }

		private async Task<int> PointsCheck(int playerId, int playerPoints)
		{
			if (playerPoints < Constant.MinPointsValueToPlay)
			{
				await RestorePoints(playerId);
				playerPoints = Constant.DefaultPointsValue;
			}

			return playerPoints;
		}
    }
}
