using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration.Constant;
using BlackJack.Entity;
using BlackJack.DAL.Interface;

namespace BlackJack.DAL.Repository
{
    public class HandRepository : IHandRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task<IEnumerable<int>> GetIdCardsByPlayerId(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT CardId FROM Hand WHERE PlayerId = {playerId}";
                var cards = await db.QueryAsync<int>(sqlQuery);
                return cards;
            }
        }

        public async Task GiveCardToPlayer(int playerId, int cardId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Hand (PlayerId, CardId) VALUES({playerId}, {cardId})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task RemoveAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM Hand WHERE CardId > 0";
                await db.ExecuteAsync(sqlQuery);
            }
        }
    }
}
