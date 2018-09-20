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

			//using (var db = new SqlConnection(_connectionString))
			//{
			//	var sqlQuery = "SELECT * FROM LogInfo INNER JOIN Player on LogInfo.PlayerId = Player.Id ";
			//	Game currentGame = (await db.QueryAsync<Game, Player, Game>(sqlQuery,
			//	(game, player) =>
			//	{
			//		game.Human = player;
			//		return game;
			//	}, new { gameId }))
			//	.FirstOrDefault();
			//	return currentGame;
			//}



			return messages;
		}
	}
}