﻿using System;
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
    public class PlayerRepository : IPlayerRepository
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

        public void DeleteById(int id)
        {
            var player = _gameContext.Players.Find(id);
            var hands = _gameContext.Hands.Where(x => x.IdPlayer == id);
            if (player != null)
            {
                _gameContext.Players.Remove(player);
                _gameContext.Hands.RemoveRange(_gameContext.Hands.Where(x => x.IdPlayer == id));   
            }
        }

        public IEnumerable<Player> GetAll()
        {
            return _gameContext.Players;
        }

        public Player GetById(int id)
        {
            return _gameContext.Players.Find(id);
        }

        public void Update(Player entity)
        {
            _gameContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
