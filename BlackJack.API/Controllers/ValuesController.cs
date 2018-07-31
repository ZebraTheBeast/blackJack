﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.BLL.Providers;
using BlackJack.ViewModel;

namespace BlackJack.API.Controllers
{
    public class ValuesController : ApiController
    {
        GameProvider _gameProvider = new GameProvider();

       [HttpPost]
        public async Task<GameViewModel> StartGame([FromBody]string playerName)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.StartGame(playerName);
            return gameViewModel;
        }

        [HttpPost]
        public async Task<GameViewModel> Bet([FromBody]BetViewModel betViewModel)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.PlaceBet(betViewModel.HumanId, betViewModel.BetValue);
            return gameViewModel;
        }

        [HttpPost]
        public async Task<GameViewModel> Draw([FromBody]DrawViewModel drawViewModel)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.Draw(drawViewModel.HumanId, drawViewModel.Deck);
            return gameViewModel;
        }

        [HttpPost]
        public async Task<GameViewModel> Stand([FromBody]List<int> deck)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.BotTurn(deck);
            return gameViewModel;
        }
    }
}
