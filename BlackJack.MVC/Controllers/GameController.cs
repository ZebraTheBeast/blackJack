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
        public ActionResult Game(LoginPlayersModel loginPlayersModel, string playerName)
        {
            // TODO - vremennoe reshenie(navsegda)
            var player = new PlayerModel();
            player.Name = playerName;
            player.Id = loginPlayersModel.PlayerList.Count;
            player.Points = 3333;
            loginPlayersModel.PlayerList.Add(player);

            //_gameService.AddPlayers(loginPlayersModel.PlayerList);

            _gameService.Dealing();
            return View();
        }

        public ActionResult Game(/*GameModel gameModel*/string name)
        {
            //if(gameModel.Players.Count == 0)
            //{
            //    //gameModel = _gameService.PlayerTest();
            //    gameModel = _gameService.AddPlayers(new List<PlayerModel>());
            //    gameModel = _gameService.Dealing();
            //}
            
            var x = new GameModel();
            x = _gameService.AddPlayers(name);
            //x = _gameService.AddPlayers(name);
            return View(x);
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