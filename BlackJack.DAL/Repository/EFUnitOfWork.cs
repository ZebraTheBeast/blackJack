using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.DAL.EF;
using BlackJack.DAL.Interface;
using BlackJack.Entity;

namespace BlackJack.DAL.Repository
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private GameContext _gameContext;
        private CardRepository _cardRepository;
        private PlayerRepository _playerRepository;
        private HandRepository _handRepository;

        public EFUnitOfWork(string connectionString)
        {
            _gameContext = new GameContext(connectionString);
        }

        public ICardRepository Cards
        {
            get
            {
                if(_cardRepository == null)
                {
                    _cardRepository = new CardRepository(_gameContext);
                }
                return _cardRepository;
            }
        }

        public IPlayerRepository Players
        {
            get
            {
                if (_playerRepository == null)
                {
                    _playerRepository = new PlayerRepository(_gameContext);
                }
                return _playerRepository;
            }
        }

        public IHandRepository Hands
        {
            get
            {
                if (_handRepository == null)
                {
                    _handRepository = new HandRepository(_gameContext);
                }
                return _handRepository;
            }
        }

        public void Save()
        {
            _gameContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if(!this._disposed)
            {
                if(disposing)
                {
                    _gameContext.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
