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

        
        public ActionResult Game(GameModel gameModel)
        {
           
            return View(gameModel);
        }

        [HttpPost, ActionName("StartGame")]
        public ActionResult StartGame(PlayerModel player)
        {
            var gameModel = _gameService.StartGame(player);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("Draw")]
        public ActionResult Draw(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            var humanId = gameModel.Players.Count() - 1;
            gameModel = _gameService.GiveCard(humanId, gameModel);

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("BotTurn")]
        public ActionResult BotTurn(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            for (var i = 1; i < gameModel.Players.Count - 1; i++)
            {
                gameModel = _gameService.BotTurn(gameModel, gameModel.Players[i], 16);
            }

            gameModel.ButtonPushed = 2;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("DealerTurn")]
        public ActionResult DealerTurn(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            gameModel = _gameService.BotTurn(gameModel, gameModel.Players[0], 16);
            gameModel = _gameService.EditPoints(gameModel);

            gameModel.ButtonPushed = 3;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("EndTurn")]
        public ActionResult EndTurn(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            gameModel = _gameService.EndTurn(gameModel);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("PlaceBet")]
        public ActionResult PlaceBet(string jsonModel, int pointsValue)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            gameModel = _gameService.PlaceBet(gameModel, gameModel.Players.Count - 1, pointsValue);
            gameModel = _gameService.Dealing(gameModel);

            gameModel.ButtonPushed = 1;

            return View("Game", gameModel);
        }
    }
}