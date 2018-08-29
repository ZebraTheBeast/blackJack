using System.Web.Mvc;

namespace BlackJack.MVC.Controllers
{
	public class GameController : Controller
	{
		public ActionResult Game()
		{
			return View();
		}

		public ActionResult Logs()
		{
			return View();
		}

		public ActionResult Login()
		{
			return View();
		}
	}
}
