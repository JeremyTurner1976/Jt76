namespace Jt76.Data.Services.LoggingServices
{
	using System;
	using Microsoft.Extensions.Logging;

	public class DatabaseLoggerProvider : ILoggerProvider
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly Func<string, LogLevel, bool> _filter;

		public DatabaseLoggerProvider(Func<string, LogLevel, bool> filter, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_filter = filter;
		}

		public void Dispose()
		{
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new DatabaseLoggingService(categoryName, _filter, _serviceProvider);
		}
	}
}