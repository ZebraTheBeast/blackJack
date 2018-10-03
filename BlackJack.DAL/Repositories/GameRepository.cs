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

		public async Task DeleteGameById(long id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.DeleteAsync(new Game() { Id = id });
			}
		}

		public async Task<long> StartNewGame()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var gameId = await db.InsertAsync(new Game ());
				return gameId;
			}
		}

		public async Task<long> GetGameIdByHumanId(long humanId)
		{
			var sqlQuery = @"SELECT Game.Id FROM Game 
				INNER JOIN PlayerInGame ON Game.Id = PlayerInGame.GameId 
				WHERE PlayerId = @humanId 
				AND IsHuman = 1";
			using (var db = new SqlConnection(_connectionString))
			{
				var gameId = (await db.QueryAsync<long>(sqlQuery, new { humanId })).FirstOrDefault();
				return gameId;
			}
		}

		public async Task<Game> GetById(long gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = @"SELECT * FROM Game 
					INNER JOIN PlayerInGame ON Game.Id = PlayerInGame.GameId 
					INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id 
					WHERE Game.Id = @gameId";

				var gameDictionary = new Dictionary<long, Game>();
				var playerInGameDictionary = new Dictionary<long, PlayerInGame>();

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

				sqlQuery = @"SELECT * FROM PlayerInGame 
					INNER JOIN Hand ON PlayerInGame.Id = Hand.PlayerInGameId 
					INNER JOIN Player ON PlayerInGame.PlayerId = Player.Id 
					INNER JOIN Card ON Hand.CardId = Card.Id
					WHERE PlayerInGameId in @Ids";

				List<PlayerInGame> playersInGame = (await db.QueryAsync<PlayerInGame, Player, Hand, Card, PlayerInGame>(sqlQuery,
			   (playerInGame, player, hand, card) =>
			   {
				   PlayerInGame playerInGameResult;
				   hand.Card = card;
				   if (!playerInGameDictionary.TryGetValue(playerInGame.Id, out playerInGameResult))
				   {
					   playerInGameDictionary.Add(playerInGame.Id, playerInGameResult = playerInGame);
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
					foreach(var playerInGame in playerInGameDictionary)
					{
						currentGame.PlayersInGame.Where(player => player.PlayerId == playerInGame.Value.PlayerId).FirstOrDefault().CardsInHand = playerInGame.Value.CardsInHand;
					}
				}

				return currentGame;
			}
		}
	}
}
