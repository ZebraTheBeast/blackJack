using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlackJack.DAL.EF
{
    class GameDbInitializer : DropCreateDatabaseIfModelChanges<GameContext>
    {
        protected override void Seed(GameContext db)
        {
            db.Players.Add(new Entity.Player { Id = 1, Name = "Zebra", Points = 10000 });
            db.SaveChanges();
        }
    }
}
