using Autofac;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Providers;
using BlackJack.BusinessLogic.Services;
using BlackJack.DataAccess.Interfaces;
using BlackJack.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Utils
{
	public static class AutofacTypeConfig
	{
		public static ContainerBuilder GetBuilderTypes(ContainerBuilder builder)
		{
			builder.RegisterType<DeckProvider>().As<IDeckProvider>();
			builder.RegisterType<HandProvider>().As<IHandProvider>();
			builder.RegisterType<PlayerProvider>().As<IPlayerProvider>();
			builder.RegisterType<ScoreProvider>().As<IScoreProvider>();

			builder.RegisterType<LogService>().As<ILogService>();
			builder.RegisterType<LoginService>().As<ILoginService>();
			builder.RegisterType<GameService>().As<IGameService>();

			builder.RegisterType<HandRepository>().As<IHandRepository>();
			builder.RegisterType<PlayerInGameRepository>().As<IPlayerInGameRepository>();
			builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();
			builder.RegisterType<LogMessageRepository>().As<ILogMessageRepository>();

			return builder;
		}
	}
}
