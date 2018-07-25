using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;

namespace BlackJack.DAL.Interface
{
    public interface ICardRepository
    {
        Task Create(Card card);
        Task<Card> GetById(int cardId);
    }
}
