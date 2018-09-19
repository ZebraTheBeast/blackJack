﻿using BlackJack.DataAccess.Interfaces;
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

		public async Task<Game> GetGameByHumanId(int humanId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT * FROM Game INNER JOIN Player on Game.HumanId = Player.Id WHERE HumanId = @humanId";
				Game currentGame = (await db.QueryAsync<Game, Player, Game>(sqlQuery, 
				(game, player) =>
				{
					game.Human = player;
					return game;
				}, new { humanId }))
				.FirstOrDefault();
				return currentGame;
			}
		}

		public async Task<Game> GetGameById(int gameId)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var sqlQuery = "SELECT * FROM Game INNER JOIN Player on Game.HumanId = Player.Id WHERE Game.Id = @gameId";
				Game currentGame = (await db.QueryAsync<Game, Player, Game>(sqlQuery, 
				(game, player) =>
				{
					game.Human = player;
					return game;
				}, new { gameId }))
				.FirstOrDefault();
				return currentGame;
			}
		}
	}
}
