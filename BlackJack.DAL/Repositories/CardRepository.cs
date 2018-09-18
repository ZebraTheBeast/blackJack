﻿using BlackJack.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using BlackJack.Entities;
using System.Linq;
using System.Collections.Generic;
using Dapper;

namespace BlackJack.DataAccess.Repositories
{
	public class CardRepository : ICardRepository
	{
		private string _connectionString;

		public CardRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task FillDB(List<Card> cards)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				foreach (var card in cards)
				{
					await db.InsertAsync(card);
				}
			}
		}

		public async Task<List<Card>> GetAll()
		{
			var cards = new List<Card>();
			using (var db = new SqlConnection(_connectionString))
			{
				cards = (await db.GetAllAsync<Card>()).ToList();
			}
			return cards;
		}

		public async Task<List<Card>> GetCardsById(List<int> cardsId)
		{
			var cards = new List<Card>();
			var sqlQuery = "SELECT * FROM Card WHERE Id IN @cardsId";

			using (var db = new SqlConnection(_connectionString))
			{
				cards = (await db.QueryAsync<Card>(sqlQuery, new { cardsId })).ToList();
			}

			return cards;
		}
	}
}
