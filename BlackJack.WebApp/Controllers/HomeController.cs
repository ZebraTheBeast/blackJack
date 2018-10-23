using System.Web.Mvc;

namespace BlackJack.WebApp.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return Redirect("/dist");
        }
    }
}