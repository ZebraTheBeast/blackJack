using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
using System.Data.Entity;
using BlackJack.DAL.Interface;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BlackJack.Configuration.Constant;

namespace BlackJack.DAL.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public Player Create(Player player)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO Player (Name) VALUES('{player.Name}')";
                db.Execute(sqlQuery);

                player = GetByName(player.Name);
            }
            return player;
        }

        public IEnumerable<Player> GetBots()
        {
            var players = new List<Player>();

            using (var db = new SqlConnection(connectionString))
            {
                players = db.Query<Player>("SELECT TOP(4) * FROM Player").ToList();
            }

            foreach(var player in players)
            {
                if(player.Points <= 0 )
                {
                    player.Points = Constant.DefaultPointsValue;
                }
            }

            return players;
        }

        public Player GetByName(string name)
        {
            var player = new Player();

            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT * FROM Player WHERE Name = '{name}'";
                player = db.Query<Player>(sqlQuery).FirstOrDefault();
            }

            if (player == null)
            {
                player = new Player();
                player.Name = name;
                Create(player);
            }

            if(player.Points <= 0)
            {
                player.Points = Constant.DefaultPointsValue;
            }

            return player;
        }

        public void MakeBet(int playerId, int bet)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE Player SET Bet = {bet} WHERE Id = {playerId}";
                db.Execute(sqlQuery);
            }
        }

        public void WinPoints(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT * FROM Player WHERE playerId = {playerId}";
                var player = db.Query<Player>(sqlQuery).First();
                var playerPoints = player.Points + player.Bet;
                sqlQuery = $"UPDATE Player SET Points = {playerPoints}, Bet = 0 WHERE Id = {player.Id}";
                db.Execute(sqlQuery);
            }
        }

        public void LosePoints(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT * FROM Player WHERE playerId = {playerId}";
                var player = db.Query<Player>(sqlQuery).First();
                var playerPoints = player.Points - player.Bet;
                sqlQuery = $"UPDATE Player SET Points = {playerPoints}, Bet = 0 WHERE Id = {player.Id}";
                db.Execute(sqlQuery);
            }
        }

        public void AnnulPoints(int playerId)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"SELECT * FROM Player WHERE playerId = {playerId}";
                var player = db.Query<Player>(sqlQuery).First();
                sqlQuery = $"UPDATE Player SET Bet = 0 WHERE Id = {player.Id}";
                db.Execute(sqlQuery);
            }
        }
    }
}
