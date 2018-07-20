using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
using System.Data.Entity;
using BlackJack.DAL.Interface;


namespace BlackJack.DAL.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        public void Create(Player player)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> GetAll()
        {
            throw new NotImplementedException();
        }

        public Player GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Update(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
