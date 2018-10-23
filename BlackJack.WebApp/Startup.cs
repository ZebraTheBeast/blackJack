using System.Web.Http;
using Autofac.Integration.WebApi;
using BlackJack.BusinessLogic.Mappers;
using BlackJack.WebApp.Configs;
using Microsoft.Owin;
using Owin;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Web.Routing;

[assembly: OwinStartup(typeof(BlackJack.WebApp.Startup))]

namespace BlackJack.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperConfig.Initialize();

            var container = AutofacConfig.ConfigureContainer();
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

        }
    }
}
