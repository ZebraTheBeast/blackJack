using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BlackJack.DataAccess.Interfaces;
using Dapper;
using System.Linq;
using System;
using BlackJack.Entities;
using Dapper.Contrib.Extensions;

namespace BlackJack.DataAccess.Repositories
{
	public class HandRepository : IHandRepository
	{
		private string _connectionString;

		public HandRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<long>> GetCardsIdByPlayerId(long playerId, long gameId)
		{
			var sqlQuery = @"SELECT CardId FROM Hand 
				INNER JOIN PlayerInGame ON Hand.PlayerInGameId = PlayerInGame.Id
				WHERE PlayerInGame.PlayerId = @playerId 
				AND PlayerInGame.GameId = @gameId";

			using (var db = new SqlConnection(_connectionString))
			{
				var cards = (await db.QueryAsync<long>(sqlQuery, new { playerId, gameId })).ToList();
				return cards;
			}


		}

		public async Task<List<long>> GetCardsIdByGameId(long gameId)
		{
			var sqlQuery = @"SELECT CardId FROM Hand 
				INNER JOIN PlayerInGame ON Hand.PlayerInGameId = PlayerInGame.Id 
				WHERE PlayerInGame.GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				var cards = (await db.QueryAsync<long>(sqlQuery, new { gameId })).ToList();
				return cards;
			}


		}

		public async Task GiveCardToPlayerInGame(long playerId, long cardId, long gameId)
		{
			var sqlQuery = @"INSERT INTO Hand (CardId, PlayerInGameId, CreationDate) VALUES(@cardId,
				(SELECT Id FROM PlayerInGame WHERE GameId = @gameId AND PlayerId = @playerId), @date)";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.QueryAsync(sqlQuery, new { cardId,  gameId, playerId, date = DateTime.Now});
			}
		}

		public async Task RemoveAllCardsInHand(long gameId)
		{
			var sqlQuery = "DELETE Hand FROM Hand INNER JOIN PlayerInGame ON Hand.PlayerInGameId = PlayerInGame.Id WHERE PlayerInGame.GameId = @gameId";
			using (var db = new SqlConnection(_connectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { gameId });
			}
		}

		public async Task<Hand> GetByIdAsync(long id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var hand = await db.GetAsync<Hand>(id);
				return hand;
			}

		}
	}
}
