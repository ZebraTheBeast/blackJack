﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
namespace BlackJack.DAL.Interface
{
    public interface IPlayerRepository
    {
        Player GetById(int id);
        IEnumerable<Player> GetAll();
        void Create(Player player);
        void Update(Player player);
        void DeleteById(int id);
    }
}
