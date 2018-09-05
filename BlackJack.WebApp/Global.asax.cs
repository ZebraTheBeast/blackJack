using System.Web.Http;
using System.Web.Mvc;
using BlackJack.BusinessLogic.Mappers;
using BlackJack.WebApp.Configs;

namespace BlackJack.WebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperConfig.Initialize();
            AutofacConfig.ConfigureContainer();
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}