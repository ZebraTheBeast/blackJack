using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories
{
	public class LogMessageRepository : ILogMessageRepository
	{
		private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

		public async Task<IEnumerable<LogMessage>> GetAll()
		{
			using (var db = new SqlConnection(connectionString))
			{
				var sqlQuery = $"SELECT * FROM LogInfo";
				var messages = await db.QueryAsync<LogMessage>(sqlQuery);
				return messages;
			}
		}
	}
}
