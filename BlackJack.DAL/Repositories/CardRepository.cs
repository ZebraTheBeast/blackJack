using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Repositories
{
	public class CardRepository : ICardRepository
	{
		private string _connectionString;

		public CardRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task FillDB(List<Card> cards)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				foreach (var card in cards)
				{
					await db.InsertAsync<Card>(card);
				}
			}
		}

		public async Task<IEnumerable<Card>> GetAll()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var cards = await db.GetAllAsync<Card>();
				return cards;
			}
		}

		public async Task<Card> GetById(int cardId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var card = await db.GetAsync<Card>(cardId);
				return card;
			}
		}
	}
}
