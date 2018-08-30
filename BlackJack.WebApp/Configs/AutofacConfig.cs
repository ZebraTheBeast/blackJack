﻿using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using BlackJack.BusinessLogic.Configs;
using System.Configuration;

namespace BlackJack.WebApp.Configs
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder = AutofacTypeConfig.GetBuilderTypes(builder, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}