using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Services;

namespace BlackJack.MVC.Controllers
{
    public class HomeController : Controller
    {
        IGame _gameService;

        public HomeController(IGame gameService)
        {
            _gameService = gameService;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}