using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BlackJack.Entity;

namespace BlackJack.DAL.EF
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet <Hand> Hands { get; set; }

        static GameContext()
        {
            Database.SetInitializer<GameContext>(new GameDbInitializer());
        }

        public GameContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
