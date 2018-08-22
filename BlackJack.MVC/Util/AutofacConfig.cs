using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using BlackJack.BusinessLogic.Utils;

namespace BlackJack.MVC.Util
{
	public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			builder = AutofacTypeConfig.GetBuilderTypes(builder);

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}