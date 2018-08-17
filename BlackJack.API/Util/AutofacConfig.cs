using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Services;
using BlackJack.BLL.Providers;
using System.Web.Http;
using BlackJack.DAL.Interface;
using BlackJack.DAL.Repository;

namespace BlackJack.WebApp.Util
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<DeckService>().As<IDeckService>();
            builder.RegisterType<GameProvider>().As<IGameProvider>();
            builder.RegisterType<HandService>().As<IHandService>();
            builder.RegisterType<PlayerService>().As<IPlayerService>();
            builder.RegisterType<ScoreService>().As<IScoreService>();

            builder.RegisterType<HandRepository>().As<IHandRepository>();
            builder.RegisterType<PlayerInGameRepository>().As<IPlayerInGameRepository>();
            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}