using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.Entities;
using BlackJack.DataAccess.Interfaces;
using Dapper;
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

		public async Task<Player> Create(Player player)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"INSERT INTO Player (Name, CreationDate) VALUES('{player.Name}', '{player.CreationDate}')";
                await db.ExecuteAsync(sqlQuery);

                player = await GetByName(player.Name);

            }
            return player;
        }

        public async Task<IEnumerable<Player>> GetBots(string playerName, int botsAmount)
        {
			var players = new List<Player>();
			var dealer = await GetByName(Constant.DealerName);

			players.Add(dealer);

			using (var db = new SqlConnection(_connectionString))
            {
				var sqlQuery = $"SELECT TOP({botsAmount}) * FROM Player WHERE Name <> '{playerName}' AND Name <> '{Constant.DealerName}'";
                var bots = await db.QueryAsync<Player>(sqlQuery);
				
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
                var sqlQuery = $"SELECT * FROM Player WHERE Name = '{name}'";
                player = (await db.QueryAsync<Player>(sqlQuery)).FirstOrDefault();
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
                var sqlQuery = $"UPDATE Player SET Points = {newPointsValue} WHERE Id = {playerId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task RestorePoints(int playerId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"UPDATE Player SET Points = {Constant.DefaultPointsValue} WHERE Id = {playerId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task<Player> GetById(int id)
        {
            var player = new Player();

            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = $"SELECT * FROM Player WHERE Id = {id}";
                player = (await db.QueryAsync<Player>(sqlQuery)).FirstOrDefault();

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
