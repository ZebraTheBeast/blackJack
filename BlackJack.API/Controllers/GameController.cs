using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackJack.API.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Game()
        {
            return View();
        }
    }
}
