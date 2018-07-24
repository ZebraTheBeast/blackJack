using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackJack.BLL.Services;
using BlackJack.ViewModel;
using System.Web.Script.Serialization;
using BlackJack.Configuration.Constant;

namespace BlackJack.MVC.Controllers
{
    public class GameController : Controller
    {
        [HttpPost]
        public ActionResult StartGame(string playerName)
        {
            var gameService = new GameService();
            gameService.StartGame(playerName);
            var gameModel = gameService.GetGameViewModel();
            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost]
        public ActionResult Draw(int humanId)
        {
            var gameService = new GameService();

            gameService.HumanDrawCard(humanId);

            var gameModel = gameService.GetGameViewModel();
            gameModel.ButtonPushed = 1;

            if (gameModel.Human.Hand.CardListValue >= Constant.WinValue)
            {

                RedirectToAction("BotTurn");
            }

            return View("Game", gameModel);
        }

        [HttpPost]
        public ActionResult Stand()
        {
            
            var gameService = new GameService();
            var gameModel = gameService.GetGameViewModel();

            for (var i = 0; i > gameModel.Bots.Count(); i++)
            {
                gameService.BotTurn(gameModel.Bots[i].Id);
            }

            gameService.BotTurn(Constant.DealerId);

            gameModel = gameService.GetGameViewModel();

            for (var i = 0; i < gameModel.Bots.Count(); i++)
            {
                gameService.UpdateScore(gameModel.Bots[i].Id, gameModel.Bots[i].Hand.CardListValue, gameModel.Dealer.Hand.CardListValue);
            }

            gameService.UpdateScore(gameModel.Human.Id, gameModel.Human.Hand.CardListValue, gameModel.Dealer.Hand.CardListValue);

            gameModel = gameService.GetGameViewModel();

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost]
        public ActionResult PlaceBet(int humanId, int pointsValue)
        {
            
            var gameService = new GameService();

            gameService.EndTurn();
            gameService.MakeBet(humanId, pointsValue);
            gameService.Dealing();

            var gameModel = gameService.GetGameViewModel();

            gameModel.ButtonPushed = 1;

            if ((gameModel.Human.Hand.CardListValue >= Constant.WinValue) || (gameModel.Dealer.Hand.CardListValue >= Constant.WinValue))
            {
                RedirectToAction("BotTurn");
            }

            return View("Game", gameModel);
        }


    }
}