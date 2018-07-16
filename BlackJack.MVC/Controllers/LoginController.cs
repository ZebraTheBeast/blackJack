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

        public LoginController(IGame gameService)
        {
            _gameService = gameService;
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(PlayerModel player)
        {
            

            return View();
        }
    }
}