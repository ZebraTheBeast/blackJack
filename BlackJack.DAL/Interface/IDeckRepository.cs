using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;

namespace BlackJack.DAL.Interface
{
    public interface IDeckRepository
    {
        void Add(int cardId);
        int GetFirstCardIdAndRemove();
        List<int> GetDeck();
        void RemoveAll();
    }
}
