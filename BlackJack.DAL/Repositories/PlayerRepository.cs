using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.Entity;
using BlackJack.DAL.Interfaces;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration;

namespace BlackJack.DAL.Repositories
{
	public class PlayerRepository : IPlayerRepository
    {
		private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task<Player> Create(Player player)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Player (Name) VALUES('{player.Name}')";
                await db.ExecuteAsync(sqlQuery);

                player = await GetByName(player.Name);

            }
            return player;
        }

        public async Task<IEnumerable<Player>> GetBots(string playerName)
        {
            IEnumerable<Player> players = new List<Player>();

            using (var db = new SqlConnection(connectionString))
            {
                players = await db.QueryAsync<Player>($"SELECT TOP(4) * FROM Player WHERE Name <> '{playerName}'");
            }

            foreach (var player in players)
            {
                if (player.Points <= 0)
                {
                    player.Points = Constant.DefaultPointsValue;
                }
            }

            return players;
        }

        public async Task<Player> GetByName(string name)
        {
            var player = new Player();

            using (var db = new SqlConnection(connectionString))
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

            if (player.Points < Constant.MinPointsValueToPlay)
            {
                await RestorePoints(player.Id);
                player.Points = Constant.DefaultPointsValue;
            }

            return player;
        }

        public async Task UpdatePoints(int playerId, int newPointsValue)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE Player SET Points = {newPointsValue} WHERE Id = {playerId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task RestorePoints(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE Player SET Points = {Constant.DefaultPointsValue} WHERE Id = {playerId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task<Player> GetById(int id)
        {
            var player = new Player();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT * FROM Player WHERE Id = {id}";
                player = (await db.QueryAsync<Player>(sqlQuery)).First();

                if (player.Points < Constant.MinPointsValueToPlay)
                {
                    await RestorePoints(player.Id);
                    player.Points = Constant.DefaultPointsValue;
                }
            }

            return player;
        }
    }
}
