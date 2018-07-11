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
    public class Player : IRepository<Player>
    {
        private DbSet<Player> _players;

        public void Add(Player entity)
        {
            _players.Add(entity);
        }

        public IEnumerable<Player> GetAll()
        {
            return _players;
        }

        public Player GetById(int id)
        {
            return _players.Find(id);
        }

        public void Remove(Player entity)
        {
            _players.Remove(entity);
        }

        public void Update(Player entity)
        {
            throw new NotImplementedException();
        }
    }
}
