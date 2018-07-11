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
    public class CardRepository : ICardRepository
    {
        private GameContext _gameContext;

        public CardRepository(GameContext gameContext)
        {
            this._gameContext = gameContext;
        }

        public void Create(Card entity)
        {
            _gameContext.Cards.Add(entity);
        }

        public void DeleteById(int id)
        {
            var card = _gameContext.Cards.Find(id);
            var hands = _gameContext.Hands.Where(x => x.IdCard == id);
            if (card != null)
            {
                _gameContext.Cards.Remove(card);
                _gameContext.Hands.RemoveRange(hands);
            }
        }

        public Card Get(int id)
        {
            return _gameContext.Cards.Find(id);
        }

        public IEnumerable<Card> GetAll()
        {
            return _gameContext.Cards;
        }

        public void Update(Card entity)
        {
            _gameContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
