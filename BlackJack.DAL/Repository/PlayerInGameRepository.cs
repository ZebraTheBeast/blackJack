using System;
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
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO PlayerInGame (PlayerId) VALUES({playerId})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task AddHuman(int playerId)
        {

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO PlayerInGame (PlayerId, Humanity) VALUES({playerId}, 1)";
                await db.ExecuteAsync(sqlQuery);
            }

        }

        public async Task<IEnumerable<int>> GetBots()
        {
            IEnumerable<int> players = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 0 AND PlayerId > 1";
                players = await db.QueryAsync<int>(sqlQuery);
            }

            return players;
        }

        public async Task<IEnumerable<int>> GetAll()
        {
            IEnumerable<int> players = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame";
                players = await db.QueryAsync<int>(sqlQuery);
            }

            return players;
        }

        public async Task<int> GetHuman()
        {
            IEnumerable<int> playerId = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 1";
                playerId = await db.QueryAsync<int>(sqlQuery);
            }
            return playerId.FirstOrDefault();
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
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE PlayerInGame SET Bet = {betValue} WHERE PlayerId = {playerId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task AnnulBet(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE PlayerInGame SET Bet = 0 WHERE PlayerId = {playerId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task<int> GetBetByPlayerId(int playerId)
        {
            IEnumerable<int> betValue = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT Bet FROM PlayerInGame WHERE PlayerId = {playerId}";
                betValue = await db.QueryAsync<int>(sqlQuery);
            }
            return betValue.FirstOrDefault();
        }
    }
}
