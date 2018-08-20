using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using BlackJack.BLL.Interfaces;
using BlackJack.BLL.Services;
using BlackJack.BLL.Providers;
using System.Web.Http;
using BlackJack.BLL.Utils;

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