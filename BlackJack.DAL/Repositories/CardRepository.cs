using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;

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
