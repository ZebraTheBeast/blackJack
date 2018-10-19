using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;
using System.Collections.Generic;
using BlackJack.DataAccess.Mappers;

namespace BlackJack.DataAccess.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
	{
		private string _connectionString;

		public GameRepository(string connectionString) : base(connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task DeleteGameById(long id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.DeleteAsync(new Game() { Id = id });
			}
		}

		public async Task<long> GetGameIdByHumanId(long humanId)
		{
			var sqlQuery = @"SELECT Game.Id FROM Game 
				INNER JOIN PlayerInGame ON Game.Id = PlayerInGame.GameId 
				WHERE PlayerId = @humanId";

			using (var db = new SqlConnection(_connectionString))
			{
				var gameId = (await db.QueryAsync<long>(sqlQuery, new { humanId })).FirstOrDefault();
				return gameId;
			}
		}

        public override async Task<Game> GetById(long gameId)
        {
            var sqlQuery = @"SELECT * FROM Game 
					INNER JOIN PlayerInGame ON Game.Id = PlayerInGame.GameId 
					INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id 
					WHERE Game.Id = @gameId";
            var gameDictionary = new Dictionary<long, Game>();
            var gameMapper = new GameMapper();

            using (var db = new SqlConnection(_connectionString))
            {
                Game currentGame = (await db.QueryAsync(
                sqlQuery,
                gameMapper.GetMap(gameDictionary),
                param: new { gameId }
                )).FirstOrDefault();

                return currentGame;
            }
        }
	}
}
