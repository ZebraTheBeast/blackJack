﻿using System;
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

        public void Update(Player player)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE Player SET Points = {player.Points} WHERE Id = {player.Id}";
                db.Execute(sqlQuery);
            }
        }
    }
}
