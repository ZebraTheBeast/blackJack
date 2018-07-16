using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Services;
using BlackJack.ViewModel;

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

        public ActionResult Game()
        {

           return View();
        }
        
        [HttpPost]
        public ActionResult Draw(string name)
        {

            var x = new GameModel();
            x = _gameService.AddPlayers(name);
            //gameModel = _gameService.AddPlayers(new List<PlayerModel>());
            return View("Game", x);
        }
    }
}