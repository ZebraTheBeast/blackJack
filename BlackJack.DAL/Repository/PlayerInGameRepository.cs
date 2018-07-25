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

        public void AddPlayer(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO PlayerInGame (PlayerId) VALUES({playerId})";
                db.Execute(sqlQuery);
            }
        }

        public void AddHuman(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO PlayerInGame (PlayerId, Humanity) VALUES({playerId}, 1)";
                db.Execute(sqlQuery);
            }
        }

        public IEnumerable<int> GetBots()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 0 AND PlayerId > 1";
                var players = db.Query<int>(sqlQuery).ToList();

                return players;
            }
        }

        public IEnumerable<int> GetAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame";
                var players = db.Query<int>(sqlQuery).ToList();

                return players;
            }
        }

        public int GetHuman()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT PlayerId FROM PlayerInGame WHERE Humanity = 1";
                var playerId = db.Query<int>(sqlQuery).First();

                return playerId;
            }
        }
        
        public void RemoveAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM PlayerInGame WHERE PlayerId > 0";
                db.Execute(sqlQuery);
            }
        }

        public void RemovePlayer(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM PlayerInGame WHERE PlayerId > 0";
                db.Execute(sqlQuery);
            }
        }

        public void MakeBet(int playerId, int betValue)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE PlayerInGame SET Bet = {betValue} WHERE PlayerId = {playerId}";
                db.Execute(sqlQuery);
            }
        }

        public void AnnulBet(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE PlayerInGame SET Bet = 0 WHERE PlayerId = {playerId}";
                db.Execute(sqlQuery);
            }
        }

        public int GetBetByPlayerId(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT Bet FROM PlayerInGame WHERE PlayerId = {playerId}";
                var betValue = db.Query<int>(sqlQuery).FirstOrDefault();
                return betValue;
            }
        }
    }
}
