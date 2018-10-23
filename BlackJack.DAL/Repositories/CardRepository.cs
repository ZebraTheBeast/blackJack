using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BlackJack.Entities;
using System.Linq;
using System.Collections.Generic;
using Dapper;

namespace BlackJack.DataAccess.Repositories
{
    public class CardRepository : BaseRepository<Card>, ICardRepository
	{
		private string _connectionString;

		public CardRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}

	}
}
