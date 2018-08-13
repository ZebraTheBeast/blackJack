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
using BlackJack.Configuration;

namespace BlackJack.DAL.Repository
{
    public class PlayerInGameRepository : IPlayerInGameRepository
    {
		private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task AddPlayer(int playerId, int gameId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO PlayerInGame (PlayerId, GameId) VALUES({playerId}, {gameId})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task AddHuman(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO PlayerInGame (PlayerId, Humanity, GameId) VALUES({playerId}, 1, {playerId})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task<IEnumerable<int>> GetBots(int gameId)
        {
            IEnumerable<int> players = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 0 AND PlayerId <> 1 AND GameId = {gameId}";
                players = await db.QueryAsync<int>(sqlQuery);
            }

            return players;
        }

        public async Task<IEnumerable<int>> GetAll(int gameId)
        {
            IEnumerable<int> players = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE GameId = {gameId}";
                players = await db.QueryAsync<int>(sqlQuery);
            }
            return players;
        }

        public async Task RemoveAll(int gameId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM PlayerInGame WHERE PlayerId > 0 AND GameId = {gameId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task PlaceBet(int playerId, int betValue, int gameId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE PlayerInGame SET Bet = {betValue} WHERE PlayerId = {playerId} AND GameId = {gameId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task AnnulBet(int playerId, int gameId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE PlayerInGame SET Bet = 0 WHERE PlayerId = {playerId} AND GameId = {gameId}";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task<int> GetBetByPlayerId(int playerId, int gameId)
        {
            int betValue;
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT Bet FROM PlayerInGame WHERE PlayerId = {playerId} AND GameId = {gameId}";
                betValue = (await db.QueryAsync<int>(sqlQuery)).FirstOrDefault();
            }
            return betValue;
        }

        public async Task<bool> IsInGame(int playerId, int gameId)
        {
            int player = -1;

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE PlayerId = {playerId} AND GameId = {gameId}";
                player = (await db.QueryAsync<int>(sqlQuery)).FirstOrDefault();
            }
            
            if(player == -1)
            {
                return false;
            }

            return true;
        }
    }
}
