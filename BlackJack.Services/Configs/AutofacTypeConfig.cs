﻿using Autofac;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Services;
using BlackJack.DataAccess.Configs;

namespace BlackJack.BusinessLogic.Configs
{
	public static class AutofacTypeConfig
	{
		public static ContainerBuilder GetBuilderTypes(ContainerBuilder builder, string connectionString)
		{
			builder.RegisterType<LogService>().As<ILogService>();
			builder.RegisterType<GameService>().As<IGameService>();

			builder = AutofacDataAccessLayerTypeConfig.GetDataAccessLayerType(builder, connectionString);

			return builder;
		}
	}
}
