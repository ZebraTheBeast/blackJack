using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
using System.Data.Entity;
using BlackJack.DAL.Interface;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration.Constant;

namespace BlackJack.DAL.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task<Player> Create(Player player)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Player (Name) VALUES('{player.Name}')";
                await db.ExecuteAsync(sqlQuery);

                player = await GetByName(player.Name);

                Logger.Logger.Info($"Player with name {player.Name} was created.");
            }
            return player;
        }

        public async Task<IEnumerable<Player>> GetBots()
        {
            IEnumerable<Player> players = new List<Player>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    players = await db.QueryAsync<Player>("SELECT TOP(4) * FROM Player");

                    if (players.Count() == 0)
                    {
                        throw new Exception($"There are no players in Table Player.");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
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
                player = (await db.QueryAsync<Player>(sqlQuery)).First();
            }

            if (player == null)
            {
                player = new Player();
                player.Name = name;
                await Create(player);
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
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT Id FROM Player WHERE Id = {playerId}";
                    var player = await db.QueryAsync<int>(sqlQuery);

                    if (player.Count() == 0)
                    {
                        throw new Exception($"Player doesn't exist with Id = {playerId}");
                    }

                    sqlQuery = $"UPDATE Player SET Points = {newPointsValue} WHERE Id = {playerId}";
                    await db.ExecuteAsync(sqlQuery);
                    Logger.Logger.Info($"Players points with id = {playerId} was updated to {newPointsValue}");
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
        }

        public async Task RestorePoints(int playerId)
        {
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT Id FROM Player WHERE Id = {playerId}";
                    var player = await db.QueryAsync<int>(sqlQuery);

                    if (player.Count() == 0)
                    {
                        throw new Exception($"Player doesn't exist with Id = {playerId}");
                    }

                    sqlQuery = $"UPDATE Player SET Points = {Constant.DefaultPointsValue} WHERE Id = {playerId}";
                    await db.ExecuteAsync(sqlQuery);
                    Logger.Logger.Info($"Players points with player id = {playerId} was restored to {Constant.DefaultPointsValue}");
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
        }

        public async Task<Player> GetById(int id)
        {
            var player = new Player();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT * FROM Player WHERE Id = {id}";
                    player = (await db.QueryAsync<Player>(sqlQuery)).First();

                    if(player == null)
                    {
                        throw new Exception($"Player doesn't exist with Id = {id}");
                    }

                    if (player.Points < Constant.MinPointsValueToPlay)
                    {
                        await RestorePoints(player.Id);
                        player.Points = Constant.DefaultPointsValue;
                    }

                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
            return player;
        }
    }
}
