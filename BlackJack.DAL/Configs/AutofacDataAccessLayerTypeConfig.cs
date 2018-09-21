using Autofac;
using BlackJack.DataAccess.Interfaces;
using BlackJack.DataAccess.Repositories;

namespace BlackJack.DataAccess.Configs
{
	public static class AutofacDataAccessLayerTypeConfig
	{
		public static ContainerBuilder GetDataAccessLayerType(ContainerBuilder builder, string connectionString)
		{
			builder.RegisterType<HandRepository>().As<IHandRepository>().WithParameter("connectionString", connectionString);
			builder.RegisterType<PlayerInGameRepository>().As<IPlayerInGameRepository>().WithParameter("connectionString", connectionString);
			builder.RegisterType<PlayerRepository>().As<IPlayerRepository>().WithParameter("connectionString", connectionString);
			builder.RegisterType<LogMessageRepository>().As<ILogMessageRepository>().WithParameter("connectionString", connectionString);
			builder.RegisterType<GameRepository>().As<IGameRepository>().WithParameter("connectionString", connectionString);
			builder.RegisterType<CardRepository>().As<ICardRepository>().WithParameter("connectionString", connectionString);
			return builder;
		}
	}
}
