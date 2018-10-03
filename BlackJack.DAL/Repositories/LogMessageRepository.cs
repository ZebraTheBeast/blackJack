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

		public async Task<List<LogMessage>> GetAllMessages()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var messages = (await db.GetAllAsync<LogMessage>()).ToList();
				return messages;
			}
		}

		public async Task<LogMessage> GetById(long id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var logMessage = await db.GetAsync<LogMessage>(id);
				return logMessage;
			}
		}
	}
}