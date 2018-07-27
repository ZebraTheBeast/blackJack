using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.BLL.Providers;
using BlackJack.ViewModel;

namespace BlackJack.MVC.Controllers
{
    public class ValueController : ApiController
    {
        GameProvider _gameProvider = new GameProvider();


        public string GetTestString()
        {
            //var gameViewModel = new GameViewModel();
            //gameViewModel = await _gameProvider.StartGame("Zebra");
            return "Test";
        }

        [HttpPost]
        public async Task<GameViewModel> Bet(int humanId, int pointsValue)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.PlaceBet(humanId, pointsValue);
            return gameViewModel;
        }

        [HttpPost]
        public async Task<GameViewModel> Draw(int humanId, [FromBody]List<int> deck)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.Draw(humanId, deck);
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
