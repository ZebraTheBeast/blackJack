using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories
{
	public class LogMessageRepository : ILogMessageRepository
	{
		private string _connectionString;

		public LogMessageRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<IEnumerable<LogMessage>> GetAll()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var messages = await db.GetAllAsync<LogMessage>();
				return messages;
			}
		}
	}
}