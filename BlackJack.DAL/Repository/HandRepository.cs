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

        public IEnumerable<int> GetIdCardsByPlayerId(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT CardId FROM Hand WHERE PlayerId = {playerId}";
                var cards = db.Query<int>(sqlQuery).ToList();
                return cards;
            }
        }

        public void GiveCardToPlayer(int playerId, int cardId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Hand (PlayerId, CardId) VALUES({playerId}, {cardId})";
                db.Execute(sqlQuery);
            }
        }

        public void RemoveAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM Hand WHERE CardId > 0";
                db.Execute(sqlQuery);
            }
        }
    }
}
