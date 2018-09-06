using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using BlackJack.MVC.Configs;
using BlackJack.BusinessLogic.Mappers;

namespace BlackJack.MVC
{
	public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			AutoMapperConfig.Initialize();
			AutofacConfig.ConfigureContainer();
			AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
        }
    }
}
