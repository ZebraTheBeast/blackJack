using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Repositories
{
	public class GameRepository : IGameRepository
	{
		private string _connectionString;

		public GameRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task DeleteGameById(int id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.DeleteAsync(new Game() { Id = id });
			}
		}

		public async Task<int> StartNewGame()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var gameId = await db.InsertAsync(new Game ());
				return gameId;
			}
		}

		public async Task<int> GetGameIdByHumanId(int humanId)
		{
			var sqlQuery = @"SELECT Game.Id FROM Game 
				INNER JOIN PlayerInGame ON Game.Id = PlayerInGame.GameId 
				WHERE PlayerId = @humanId 
				AND IsHuman = 1";
			using (var db = new SqlConnection(_connectionString))
			{
				int gameId = (await db.QueryAsync<int>(sqlQuery, new { humanId })).FirstOrDefault();
				return gameId;
			}
		}

		public async Task<Game> GetGameById(int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = @"SELECT * FROM Game 
					INNER JOIN PlayerInGame ON Game.Id = PlayerInGame.GameId 
					INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id 
					WHERE Game.Id = @gameId";

				var gameDictionary = new Dictionary<int, Game>();

				Game currentGame = (await db.QueryAsync<Game, PlayerInGame, Player, Game>(
				sqlQuery,

				(game, playerInGame, player) =>
				{
					Game gameResult;
									
					playerInGame.Player = player;
					
					if (!gameDictionary.TryGetValue(game.Id, out gameResult))
					{
						gameDictionary.Add(game.Id, gameResult = game);
					}

					if (gameResult.PlayersInGame == null)
					{
						gameResult.PlayersInGame = new List<PlayerInGame>();
					}
					gameResult.PlayersInGame.Add(playerInGame);

					return gameResult;
				},
				param: new { gameId }
				)).FirstOrDefault();

				return currentGame;
			}
		}
	}
}
