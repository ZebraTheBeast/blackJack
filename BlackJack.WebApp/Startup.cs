using System.Web.Http;
using Autofac.Integration.WebApi;
using BlackJack.BusinessLogic.Mappers;
using BlackJack.WebApp.Configs;
using Microsoft.Owin;
using Owin;

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
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}
