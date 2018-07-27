using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using System.Web.Script.Serialization;
using BlackJack.Configuration.Constant;
using BlackJack.BLL.Providers;
namespace BlackJack.MVC.Controllers
{
    public class GameController : Controller
    {

        public ActionResult Game()
        {
            return View("Game");
        }

    }
}