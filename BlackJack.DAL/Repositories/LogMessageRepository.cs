using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

		public async Task<List<LogMessage>> GetAll()
		{
			var messages = new List<LogMessage>();
			using (var db = new SqlConnection(_connectionString))
			{
				messages = (await db.GetAllAsync<LogMessage>()).ToList();

			}

			return messages;
		}
	}
}