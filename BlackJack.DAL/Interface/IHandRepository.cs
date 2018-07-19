using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;

namespace BlackJack.DAL.Interface
{
    public interface IHandRepository
    {
        IEnumerable<Card> GetByPlayerId(int id);
        void Create(Hand hand);
        void DeleteByPlayerId(int id);
        void DeleteByCardId(int id);
    }
}
