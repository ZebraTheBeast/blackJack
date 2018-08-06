﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
namespace BlackJack.BLL.Interface
{
    public interface IHandService
    {
        Task<HandViewModel> GetPlayerHand(int playerId, int gameId);
        Task<int> GetPlayerHandValue(int playerId, int gameId);
        Task RemoveAllCardsInHand(int gameId);
    }
}
