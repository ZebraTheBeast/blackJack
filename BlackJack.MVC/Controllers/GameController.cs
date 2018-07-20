using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackJack.BLL.Services;
using BlackJack.ViewModel;
using System.Web.Script.Serialization;

namespace BlackJack.MVC.Controllers
{
    public class GameController : Controller
    { 
        [HttpPost, ActionName("StartGame")]
        public ActionResult StartGame(string playerName)
        {
            var gameModel = GameService.StartGame(playerName);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("Draw")]
        public ActionResult Draw(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            var humanId = gameModel.Players.Count() - 1;
            gameModel = GameService.GiveCard(humanId, gameModel);
            if (gameModel.Players[gameModel.Players.Count - 1].Hand.CardListValue >= 21)
            {
                var newJsonModel = new JavaScriptSerializer().Serialize(gameModel);
                return BotTurn(newJsonModel);
            }
            return View("Game", gameModel);
        }

        [HttpPost, ActionName("BotTurn")]
        public ActionResult BotTurn(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            for (var i = 1; i < gameModel.Players.Count - 1; i++)
            {
                gameModel = GameService.BotTurn(gameModel, gameModel.Players[i], 16);
            }

            gameModel = GameService.BotTurn(gameModel, gameModel.Players[0], 16);

            gameModel = GameService.EditPoints(gameModel);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("PlaceBet")]
        public ActionResult PlaceBet(string jsonModel, int pointsValue)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
            gameModel = GameService.EndTurn(gameModel);
            gameModel = GameService.PlaceBet(gameModel, gameModel.Players.Count - 1, pointsValue);
            gameModel = GameService.Dealing(gameModel);
            
            gameModel.ButtonPushed = 1;
            if(gameModel.Players[gameModel.Players.Count-1].Hand.CardListValue >= 21)
            {
                var newJsonModel = new JavaScriptSerializer().Serialize(gameModel);
                return BotTurn(newJsonModel);
            }
            return View("Game", gameModel);
        }

        [HttpPost, ActionName("RefreshGame")]
        public ActionResult RefreshGame(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);

            gameModel = GameService.StartGame(gameModel.Players[gameModel.Players.Count - 1]);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }
    }
}