using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using System.Web.Script.Serialization;
using BlackJack.Configuration.Constant;
using BlackJack.BLL.Helper;

namespace BlackJack.MVC.Controllers
{
    public class GameController : Controller
    {
        IGameHelper _gameHelper;

        public GameController(IGameHelper gameService)
        {
            _gameHelper = gameService;
        }

        [HttpPost]
        public async Task<ActionResult> StartGame(string playerName)
        {
            var gameViewModel = new GameViewModel();
            
            gameViewModel = await _gameHelper.StartGame(playerName);
            
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Draw(int humanId, string jsonDeck)
        {
            var deck = new JavaScriptSerializer().Deserialize<List<int>>(jsonDeck);
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameHelper.Draw(humanId, deck);
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Stand(string  jsonDeck)
        {
            var deck = new JavaScriptSerializer().Deserialize<List<int>>(jsonDeck);
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameHelper.BotTurn(deck);
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBet(int humanId, int pointsValue)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameHelper.PlaceBet(humanId, pointsValue);
            return View("Game", gameViewModel);
        }
        // asinchron, exception
        

    }
}