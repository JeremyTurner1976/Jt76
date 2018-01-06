namespace Jt76.Data.Services.LoggingServices
{
	using System;
	using Microsoft.Extensions.Logging;

	public static class DatabaseLoggerFactoryExtensions
	{
		public static ILoggerFactory AddDatabaseLogger(
			this ILoggerFactory factory,
			IServiceProvider serviceProvider,
			Func<string, LogLevel, bool> filter = null)
		{
			factory.AddProvider(new DatabaseLoggerProvider(filter, serviceProvider));
			return factory;
		}

		public static ILoggerFactory AddDatabaseLogger(
			this ILoggerFactory factory,
			IServiceProvider serviceProvider,
			LogLevel minLevel)
		{
			return AddDatabaseLogger(
				factory,
				serviceProvider,
				(_, logLevel) => logLevel >= minLevel);
		}
	}
}