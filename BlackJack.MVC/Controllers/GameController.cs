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
using BlackJack.BLL.Providers;
namespace BlackJack.MVC.Controllers
{
    public class GameController : Controller
    {
        GameProvider _gameProvider;

        public GameController()
        {
            _gameProvider = new GameProvider();
        }

        [HttpPost]
        public async Task<ActionResult> StartGame(string playerName)
        {
            var gameViewModel = new GameViewModel();
            
            gameViewModel = await _gameProvider.StartGame(playerName);
            
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Draw(int humanId, string jsonDeck)
        {
            var deck = new JavaScriptSerializer().Deserialize<List<int>>(jsonDeck);
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.Draw(humanId, deck);
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Stand(string  jsonDeck)
        {
            var deck = new JavaScriptSerializer().Deserialize<List<int>>(jsonDeck);
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.BotTurn(deck);
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBet(int humanId, int pointsValue)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.PlaceBet(humanId, pointsValue);
            return View("Game", gameViewModel);
        }
    }
}