using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration.Constant;
using BlackJack.Entity;
using BlackJack.DAL.Interface;
using BlackJack.Logger;
namespace BlackJack.DAL.Repository
{
    public class HandRepository : IHandRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task<IEnumerable<int>> GetIdCardsByPlayerId(int playerId)
        {
            IEnumerable<int> cards = new List<int>();
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT CardId FROM Hand WHERE PlayerId = {playerId}";
                    cards = await db.QueryAsync<int>(sqlQuery);

                    if (cards.Count() == 0)
                    {
                        throw new Exception($"Cards not found with playerId = {playerId}.");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Warning($"{exception.Source} {exception.Message} ");
            }

            return cards;
        }

        public async Task GiveCardToPlayer(int playerId, int cardId)
        {
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    var sqlQuery = $"SELECT Id FROM Player WHERE Id = {playerId}";
                    var player = await db.QueryAsync<int>(sqlQuery);

                    if(player.Count() == 0)
                    {
                        throw new Exception($"Player with id {playerId} not found.");
                    }

                    sqlQuery = $"SELECT Id FROM Card WHERE Id = {cardId}";
                    var card = await db.QueryAsync<int>(sqlQuery);

                    if (card.Count() == 0)
                    {
                        throw new Exception($"Card with id {cardId} not found.");
                    }

                    sqlQuery = $"INSERT INTO Hand (PlayerId, CardId) VALUES({playerId}, {cardId})";
                    await db.ExecuteAsync(sqlQuery);

                    Logger.Logger.Info($"Player with id = {playerId} draw card with id = {cardId}.");
                }
            }
            catch (Exception exception)
            {
                Logger.Logger.Error($"{exception.Source} {exception.Message}");
            }
        }

        public async Task RemoveAll()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM Hand WHERE CardId > 0";
                await db.ExecuteAsync(sqlQuery);
                Logger.Logger.Info($"Removed all cards from hands.");
            }
        }
    }
}
