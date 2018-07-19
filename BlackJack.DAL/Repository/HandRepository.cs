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
    public class HandRepository : IHandRepository
    {
        private GameContext _gameContext;

        public HandRepository(GameContext gameContext)
        {
            this._gameContext = gameContext;
        }

        public void Create(Hand hand)
        {
            _gameContext.Hands.Add(hand);
        }

        public void DeleteByCardId(int id)
        {
            var hands = _gameContext.Hands.Where(x => x.IdCard == id);
            _gameContext.Hands.RemoveRange(hands);
        }

        public void DeleteByPlayerId(int id)
        {
            var hands = _gameContext.Hands.Where(x => x.IdPlayer == id);
            _gameContext.Hands.RemoveRange(hands);
        }

        public IEnumerable<Card> GetByPlayerId(int id)
        {
            var hand = _gameContext.Hands.Where(h => h.IdPlayer == id);
            var cards = new List<Card>();
            foreach (var card in hand)
            {
                cards.Add(_gameContext.Cards.First(c => c.Id == card.IdCard));
            }
            return cards;
        }
    }
}
