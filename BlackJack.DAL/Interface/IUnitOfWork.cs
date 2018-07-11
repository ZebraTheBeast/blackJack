using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;

namespace BlackJack.DAL.Interface
{
    interface IUnitOfWork : IDisposable
    {
        IRepository<Player> Players { get; }
        IRepository<Card> Cards { get; }
        IRepository<Hand> Hands { get; }

        void Save();
    }
}
