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
            var gameViewModel = gameService.GetGameViewModel();
            gameViewModel.ButtonPushed = 0;

            gameViewModel.Options = OptionService.OptionSetBet("");

            return View("Game", gameViewModel);
        }

        [HttpPost]
        public ActionResult Draw(int humanId)
        {
            var gameService = new GameService();

            gameService.HumanDrawCard(humanId);

            var gameViewModel = gameService.GetGameViewModel();
            gameViewModel.ButtonPushed = 1;
            gameViewModel.Options = OptionService.OptionDrawCard();

            if (gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
            {
                gameViewModel = BotTurn();
            }

            return View("Game", gameViewModel);
        }

        [HttpPost]
        public ActionResult Stand()
        {
            var gameViewModel = BotTurn();
            return View("Game", gameViewModel);
        }

        [HttpPost]
        public ActionResult PlaceBet(int humanId, int pointsValue)
        {
            
            var gameService = new GameService();

            gameService.EndTurn();
            gameService.MakeBet(humanId, pointsValue);
            gameService.Dealing();

            var gameViewModel = gameService.GetGameViewModel();

            gameViewModel.ButtonPushed = 1;
            gameViewModel.Options = OptionService.OptionDrawCard();

            if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue) || (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
            {
                gameViewModel = BotTurn();
            }

            return View("Game", gameViewModel);
        }

        private GameViewModel BotTurn()
        {
            var gameService = new GameService();
            var gameViewModel = gameService.GetGameViewModel();

            for (var i = 0; i > gameViewModel.Bots.Count(); i++)
            {
                gameService.BotTurn(gameViewModel.Bots[i].Id);
            }

            gameService.BotTurn(Constant.DealerId);

            gameViewModel = gameService.GetGameViewModel();

            for (var i = 0; i < gameViewModel.Bots.Count(); i++)
            {
                gameService.UpdateScore(gameViewModel.Bots[i].Id, gameViewModel.Bots[i].Hand.CardListValue, gameViewModel.Dealer.Hand.CardListValue);
            }

            var message = gameService.UpdateScore(gameViewModel.Human.Id, gameViewModel.Human.Hand.CardListValue, gameViewModel.Dealer.Hand.CardListValue);

            gameViewModel = gameService.GetGameViewModel();
            gameViewModel.Options = message;
            gameViewModel.Options = OptionService.OptionSetBet(message);

            gameViewModel.ButtonPushed = 0;
            return gameViewModel;
        }

    }
}