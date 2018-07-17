using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Services;
using BlackJack.ViewModel;
using System.Web.Script.Serialization;

namespace BlackJack.MVC.Controllers
{
    public class GameController : Controller
    {
        IGame _gameService;

        public GameController(IGame gameService)
        {
            _gameService = gameService;
          
        }

        [HttpPost]
        public ActionResult Game(PlayerModel player)
        {
            var gameModel = new GameModel();
            gameModel = _gameService.PlayerTest(gameModel, player);
            gameModel = _gameService.Dealing(gameModel);
            return View(gameModel);
        }

        public ActionResult Game(GameModel gameModel)
        {
           return View(gameModel);
        }
        
        [HttpPost]
        public ActionResult Draw(string json, int playerId)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(json);
            gameModel = _gameService.GiveCard(playerId, gameModel);
            return View("Game", gameModel);
        }
    }
}