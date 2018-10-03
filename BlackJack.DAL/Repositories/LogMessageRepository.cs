using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Repositories
{
	public class LogMessageRepository : GenericRepository<LogMessage>, ILogMessageRepository
	{
		private string _connectionString;

		public LogMessageRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}
	}
}