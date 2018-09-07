using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.WebApi;
using BlackJack.BusinessLogic.Mappers;
using BlackJack.MVC.Configs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BlackJack.MVC.Startup))]

namespace BlackJack.MVC
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			AutoMapperConfig.Initialize();
			
			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			var container = AutofacConfig.ConfigureContainer();

			var config = new HttpConfiguration();

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
