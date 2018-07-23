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
    public class CardRepository : ICardRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Create(Card card)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Card (Id, Title, Color, Value) VALUES({card.Id}, '{card.Title}', '{card.Color}', {card.Value})";
                db.Execute(sqlQuery);
            }
        }

        public Card GetById(int cardId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT * FROM Card WHERE Id = {cardId}";
                var card = db.Query<Card>(sqlQuery).First();

                return card;
            }
        }
    }
}
