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

        public async Task Create(Card card)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Card (Id, Title, Color, Value) VALUES({card.Id}, '{card.Title}', '{card.Color}', {card.Value})";
                await db.ExecuteAsync(sqlQuery);
            }
        }

        public async Task<Card> GetById(int cardId)
        {
            IEnumerable<Card> card = new List<Card>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT * FROM Card WHERE Id = {cardId}";
                    card = await db.QueryAsync<Card>(sqlQuery);

                    if (card.Count() == 0)
                    {
                        throw new Exception($"Card not found wit Id = {cardId}");
                    }
                }
            }
            catch(Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }

            return card.First();
        }
    }
}
