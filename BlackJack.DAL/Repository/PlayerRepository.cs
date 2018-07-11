using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
using System.Data.Entity;
using BlackJack.DAL.Interface;
using BlackJack.DAL.EF;

namespace BlackJack.DAL.Repository
{
    public class PlayerRepository : IRepository<Player>
    {
        private GameContext _gameContext;

        public PlayerRepository(GameContext gameContext)
        {
            this._gameContext = gameContext;
        }

        public void Create(Player entity)
        {
            _gameContext.Players.Add(entity);
        }

        public void Delete(int id)
        {
            Player player = _gameContext.Players.Find(id);
            var hands = _gameContext.Hands.Where(x => x.IdPlayer == id);
            if (player != null)
            {
                _gameContext.Players.Remove(player);
                _gameContext.Hands.RemoveRange(_gameContext.Hands.Where(x => x.IdPlayer == id));   
            }
        }

        public Player Get(int id)
        {
            return _gameContext.Players.Find(id);
        }

        public IEnumerable<Player> GetAll()
        {
            return _gameContext.Players;
        }

        public void Update(Player entity)
        {
            _gameContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
