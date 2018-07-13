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
    public class LoginController : Controller
    {
        IGame _gameService;
        // TODO - snachala ebashim vhod, a s botami pohui

        public LoginController(IGame gameService)
        {
            _gameService = gameService;
        }

        public ActionResult Login()
        {
            var loginPlayersModel = _gameService.GetStartPlayers();
            return View(loginPlayersModel);
        }


        [HttpPost]
        public ActionResult Login(LoginPlayersModel loginPlayersModel)
        {
            loginPlayersModel.PlayerList.Add(loginPlayersModel.Player);
            return View(loginPlayersModel);
        }
    }
}