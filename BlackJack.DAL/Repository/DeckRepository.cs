using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.DAL.Interface;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration.Constant;
using BlackJack.Entity;

namespace BlackJack.DAL.Repository
{
    class DeckRepository : IDeckRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Add(int cardId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Deck (CardId) VALUES({cardId})";
                db.Execute(sqlQuery);
            }
        }

        public int GetFirstCardIdAndRemove()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT TOP(1) * FROM Deck";
                var card = db.Query<Deck>(sqlQuery).First();

                sqlQuery = $"DELETE FROM Deck WHERE Id = {card.CardId}";
                db.Execute(sqlQuery);

                return card.CardId;
            }
        }

        public List<int> GetDeck()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT CardId FROM Deck";
                var deck = db.Query<int>(sqlQuery).ToList();
                
                return deck;
            }
        }

        public void RemoveAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM Deck WHERE CardId > 0";
                db.Execute(sqlQuery);
            }
        }
    }
}
