﻿using Autofac;
using Autofac.Integration.WebApi;
using Backend.WeChatApp.Repository.Core;
using Backend.WeChatApp.Repository.SqlServer;
using Backend.WeChatApp.Service;
using Backend.WeChatApp.Service.Interfaces;
using System.Reflection;
using System.Web.Http;

namespace Backend.WeChatApp.API.Config
{
	public class AutofacWebAPI
	{
		public static void Initialize(HttpConfiguration config)
		{
			Initialize(config, RegisterServices(new ContainerBuilder()));
		}

		public static void Initialize(HttpConfiguration config, IContainer container)
		{
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}

		private static IContainer RegisterServices(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

			// dbsession
			builder.RegisterType<DbSession>().As<IDbSession>().InstancePerRequest();

			// repository
			builder.RegisterGeneric(typeof(SqlRepositoryBase<>)).As(typeof(ISqlRepository<>)).InstancePerRequest();

			// service
			builder.RegisterType<CryptoService>().As<ICryptoService>().InstancePerRequest();

			return builder.Build();
		}
	}
}