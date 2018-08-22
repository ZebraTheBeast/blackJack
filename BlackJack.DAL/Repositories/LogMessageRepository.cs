using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using Dapper;
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
				var sqlQuery = $"SELECT * FROM LogInfo";
				var messages = await db.QueryAsync<LogMessage>(sqlQuery);
				return messages;
			}
		}
	}
}