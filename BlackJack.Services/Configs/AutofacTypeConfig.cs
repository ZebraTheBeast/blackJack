using Autofac;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Providers;
using BlackJack.BusinessLogic.Services;
using BlackJack.DataAccess.Configs;

namespace BlackJack.BusinessLogic.Configs
{
	public static class AutofacTypeConfig
	{
		public static ContainerBuilder GetBuilderTypes(ContainerBuilder builder, string connectionString)
		{
			builder.RegisterType<LogService>().As<ILogService>();
			builder.RegisterType<LoginService>().As<ILoginService>();
			builder.RegisterType<GameService>().As<IGameService>();
			builder.RegisterType<CardProvider>().As<ICardProvider>();

			builder = AutofacDataAccessLayerTypeConfig.GetDataAccessLayerType(builder, connectionString);

			return builder;
		}
	}
}
