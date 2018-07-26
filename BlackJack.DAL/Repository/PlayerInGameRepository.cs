﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.DAL.Interface;
using BlackJack.Entity;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration.Constant;

namespace BlackJack.DAL.Repository
{
    public class PlayerInGameRepository : IPlayerInGameRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task AddPlayer(int playerId)
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

                    sqlQuery = $"INSERT INTO PlayerInGame (PlayerId) VALUES({playerId})";
                    await db.ExecuteAsync(sqlQuery);
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
        }

        public async Task AddHuman(int playerId)
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

                    sqlQuery = $"INSERT INTO PlayerInGame (PlayerId, Humanity) VALUES({playerId}, 1)";
                    await db.ExecuteAsync(sqlQuery);
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
        }

        public async Task<IEnumerable<int>> GetBots()
        {
            IEnumerable<int> players = new List<int>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 0 AND PlayerId > 1";
                    players = await db.QueryAsync<int>(sqlQuery);

                    if (players.Count() == 0)
                    {
                        throw new Exception("Bots not in game.");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
            return players;
        }

        public async Task<IEnumerable<int>> GetAll()
        {
            IEnumerable<int> players = new List<int>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT PlayerId FROM PlayerInGame";
                    players = await db.QueryAsync<int>(sqlQuery);

                    if (players.Count() == 0)
                    {
                        throw new Exception("There are no players in game.");
                    }


                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }

            return players;
        }

        public async Task<int> GetHuman()
        {
            IEnumerable<int> playerId = new List<int>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 1";
                    playerId = await db.QueryAsync<int>(sqlQuery);

                    if (playerId.Count() == 0)
                    {
                        throw new Exception("There are no human in game.");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }

            return playerId.First();
        }

        public async Task RemoveAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM PlayerInGame WHERE PlayerId > 0";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task MakeBet(int playerId, int betValue)
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

                    sqlQuery = $"UPDATE PlayerInGame SET Bet = {betValue} WHERE PlayerId = {playerId}";
                    await db.ExecuteAsync(sqlQuery);

                    Logger.Logger.Info($"Player with id {playerId} make bet with value {betValue}");
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }

        }

        public async Task AnnulBet(int playerId)
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

                    sqlQuery = $"UPDATE PlayerInGame SET Bet = 0 WHERE PlayerId = {playerId}";
                    await db.ExecuteAsync(sqlQuery);

                    Logger.Logger.Info($"Player bet with id = {playerId} was reset to 0.");
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
        }

        public async Task<int> GetBetByPlayerId(int playerId)
        {
            IEnumerable<int> betValue = new List<int>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT Bet FROM PlayerInGame WHERE PlayerId = {playerId}";
                    betValue = await db.QueryAsync<int>(sqlQuery);

                    if (betValue.Count() == 0)
                    {
                        throw new Exception($"Player doesn't exist with Id = {playerId}");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
            return betValue.First();
        }
    }
}
