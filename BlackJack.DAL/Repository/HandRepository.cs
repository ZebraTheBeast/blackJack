using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration;
using BlackJack.Entity;
using BlackJack.DAL.Interface;

namespace BlackJack.DAL.Repository
{
    public class HandRepository : IHandRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task<IEnumerable<int>> GetIdCardsByPlayerId(int playerId, int gameId)
        {
            IEnumerable<int> cards = new List<int>();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT CardId FROM Hand WHERE PlayerId = {playerId} AND GameId = {gameId}";
                cards = await db.QueryAsync<int>(sqlQuery);
            }


            return cards;
        }

        public async Task GiveCardToPlayer(int playerId, int cardId, int gameId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Hand (PlayerId, CardId, GameId) VALUES({playerId}, {cardId}, {gameId})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task RemoveAll(int gameId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM Hand WHERE CardId > 0 AND GameId = {gameId}";
                await db.ExecuteAsync(sqlQuery);
                
            }
        }
    }
}
