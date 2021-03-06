﻿using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IGameService
    {
        Task<ResponseBetGameView> PlaceBet(RequestBetGameView requestBetGameViewModel);
        Task<DrawGameView> DrawCard(long humanId);
        Task<StandGameView> Stand(long humanId);
        Task<ResponseStartMatchGameView> StartGame(string playerName, int botsAmount);
        Task<LoadMatchGameView> LoadGame(string playerName);
    }
}
