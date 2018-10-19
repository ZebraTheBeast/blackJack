using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BlackJack.Entities;
using System.Linq;
using System.Collections.Generic;
using Dapper;

namespace BlackJack.DataAccess.Repositories
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
	{
		private string _connectionString;

		public CardRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<Card>> GetCardsById(List<long> cardsId)
		{
			var sqlQuery = "SELECT * FROM Card WHERE Id IN @cardsId";

			using (var db = new SqlConnection(_connectionString))
			{
				var cards = (await db.QueryAsync<Card>(sqlQuery, new { cardsId })).ToList();
				return cards;
			}
		}
	}
}
