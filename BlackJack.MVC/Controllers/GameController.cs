﻿using System;
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
            var gameModel = new JavaScriptSerializer().Deserialize<GameViewModel>(jsonModel);
            var humanId = gameModel.Players.Last().Id;
            gameModel = GameService.GiveCard(humanId, gameModel);
            if (gameModel.Players[gameModel.Players.Count - 1].Hand.CardListValue >= Constant.WinValue)
            {
                var newJsonModel = new JavaScriptSerializer().Serialize(gameModel);
                return BotTurn(newJsonModel);
            }
            return View("Game", gameModel);
        }

        [HttpPost, ActionName("BotTurn")]
        public ActionResult BotTurn(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameViewModel>(jsonModel);

            for (var i = gameModel.Players.Count - 2; i > -1; i--)
            {
                gameModel = GameService.BotTurn(gameModel, gameModel.Players[i], Constant.ValueToStopDraw);
            }

            gameModel = GameService.EditPoints(gameModel);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }

        [HttpPost, ActionName("PlaceBet")]
        public ActionResult PlaceBet(string jsonModel, int pointsValue)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameViewModel>(jsonModel);
            gameModel = GameService.EndTurn(gameModel);
            gameModel = GameService.PlaceBet(gameModel, gameModel.Players.Last().Id, pointsValue);
            gameModel = GameService.Dealing(gameModel);

            gameModel.ButtonPushed = 1;
            if ((gameModel.Players[gameModel.Players.Count - 1].Hand.CardListValue >= Constant.WinValue) || (gameModel.Players[0].Hand.CardListValue >= Constant.WinValue))
            {
                var newJsonModel = new JavaScriptSerializer().Serialize(gameModel);
                return BotTurn(newJsonModel);
            }
            return View("Game", gameModel);
        }

        [HttpPost, ActionName("RefreshGame")]
        public ActionResult RefreshGame(string jsonModel)
        {
            var gameModel = new JavaScriptSerializer().Deserialize<GameViewModel>(jsonModel);

            gameModel = GameService.StartGame(gameModel.Players[gameModel.Players.Count - 1].Name);

            gameModel.ButtonPushed = 0;

            return View("Game", gameModel);
        }
    }
}