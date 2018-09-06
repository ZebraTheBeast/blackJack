using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Repositories
{
	public class GameRepository : IGameRepository
	{
		private string _connectionString;

		public GameRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<int> GetGameIdByHumanId(int humanId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT Id FROM Game WHERE HumanId = @humanId";
				var gameId = (await db.QueryAsync<int>(sqlQuery, new { humanId })).FirstOrDefault();
				return gameId;
			}
		}

		public async Task Delete(int id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.DeleteAsync(new Game() { Id = id });
			}
		}

		public async Task Create(int humanId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.InsertAsync(new Game { HumanId = humanId });
			}
		}
	}
}
