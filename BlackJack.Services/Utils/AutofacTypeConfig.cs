﻿using Autofac;
using BlackJack.BLL.Interfaces;
using BlackJack.BLL.Providers;
using BlackJack.BLL.Services;
using BlackJack.DAL.Interfaces;
using BlackJack.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BLL.Utils
{
	public static class AutofacTypeConfig
	{
		public static ContainerBuilder GetBuilderTypes(ContainerBuilder builder)
		{
			builder.RegisterType<DeckService>().As<IDeckService>();
			builder.RegisterType<GameProvider>().As<IGameProvider>();
			builder.RegisterType<HandService>().As<IHandService>();
			builder.RegisterType<PlayerService>().As<IPlayerService>();
			builder.RegisterType<ScoreService>().As<IScoreService>();
			builder.RegisterType<LogService>().As<ILogService>();

			builder.RegisterType<HandRepository>().As<IHandRepository>();
			builder.RegisterType<PlayerInGameRepository>().As<IPlayerInGameRepository>();
			builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();
			builder.RegisterType<LogMessageRepository>().As<ILogMessageRepository>();

			return builder;
		}
	}
}
