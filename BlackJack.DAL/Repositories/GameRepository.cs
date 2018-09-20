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

		public async Task<int> GetGameIdByHumanId(int humanId)
		{
			var sqlQuery = "SELECT Id FROM Game WHERE HumanId = @humanId";
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
				var sqlQuery = "SELECT * FROM [Game] " +
					"INNER JOIN [PlayerInGame] ON [Game].Id = [PlayerInGame].GameId " +
					"INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id " +
					"WHERE [Game].Id = @gameId";

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

				sqlQuery = "SELECT * FROM PlayerInGame " +
					"INNER JOIN Hand ON PlayerInGame.Id = Hand.PlayerInGameId " +
					"INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id " +
					"INNER JOIN Card ON Hand.CardId = Card.Id " +
					"WHERE PlayerInGameId in @Ids";

				var playerInGameDictionary = new Dictionary<int, PlayerInGame>();

				List<PlayerInGame> playersInGame = (await db.QueryAsync<PlayerInGame, Player, Hand, Card, PlayerInGame>(sqlQuery,
				(playerInGame, player, hand, card) =>
				{
					PlayerInGame playerInGameResult;

					hand.Card = card;

					if (!playerInGameDictionary.TryGetValue(playerInGame.Id, out playerInGameResult))
					{
						playerInGameDictionary.Add(playerInGame.Id, playerInGameResult = playerInGame);
						playerInGameResult.Player = player;
					}

					if (playerInGameResult.CardsInHand == null)
					{
						playerInGameResult.CardsInHand = new List<Hand>();
					}

					playerInGameResult.CardsInHand.Add(hand);

					return playerInGameResult;
				},
				param: new { Ids = currentGame.PlayersInGame.Select(p => p.Id) }
				)).ToList();

				if (playerInGameDictionary.Count() != 0)
				{
					currentGame.PlayersInGame = playerInGameDictionary.Values.ToList();
				}

				return currentGame;
			}
		}
	}
}
