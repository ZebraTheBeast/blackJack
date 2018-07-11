using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;

namespace BlackJack.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository Players { get; }
        ICardRepository Cards { get; }
        IHandRepository Hands { get; }

        void Save();
    }
}
