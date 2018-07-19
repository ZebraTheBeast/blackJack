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

        [HttpPost]
        public ActionResult Game(string action, PlayerModel player, string jsonModel, int? pointsValue)
        {
            
            var gameModel = new GameModel();
            
            if (Request.Form["Draw"] != null)
            {
                gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
                var humanId = gameModel.Players.Count() - 1;
                gameModel = Draw(gameModel, humanId);
            }

            if(Request.Form["Play"] != null)
            {
                gameModel = _gameService.PlayerTest(gameModel, player);
                gameModel.Deck = new List<CardModel>();
                gameModel.ButtonPushed = 0;
            }

            if(Request.Form["BotTurn"] != null)
            {
                gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
                for (var i = 1; i < gameModel.Players.Count - 1; i++)
                { 
                    gameModel = _gameService.BotTurn(gameModel, gameModel.Players[i], 16);
                }
                gameModel.ButtonPushed = 2;
            }

            if (Request.Form["DealerTurn"] != null)
            {
                gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
                gameModel = _gameService.BotTurn(gameModel, gameModel.Players[0], 16);
                gameModel = _gameService.EditPoints(gameModel);
                gameModel.ButtonPushed = 3;
            }

            if (Request.Form["EndTurn"] != null)
            {
                gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
                gameModel = _gameService.EndTurn(gameModel);
                gameModel.ButtonPushed = 0;
            }
            if (Request.Form["PlaceBet"] != null)
            {
                gameModel = new JavaScriptSerializer().Deserialize<GameModel>(jsonModel);
                gameModel = _gameService.PlaceBet(gameModel, 4, pointsValue ?? 0);
                gameModel = _gameService.Dealing(gameModel);
                gameModel.ButtonPushed = 1;
            }

            return View(gameModel);
        }
        
        public GameModel Draw(GameModel gameModel, int playerId)
        {
            gameModel = _gameService.GiveCard(playerId, gameModel);
            return gameModel;
        }
    }
}