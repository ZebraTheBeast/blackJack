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
    public class HandRepository : IRepository<Hand>
    {
        private GameContext _gameContext;

        public HandRepository(GameContext gameContext)
        {
            this._gameContext = gameContext;
        }

        public void Create(Hand entity)
        {
            _gameContext.Hands.Add(entity);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Hand Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hand> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Hand entity)
        {
            throw new NotImplementedException();
        }
    }
}
