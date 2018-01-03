namespace Jt76.Common.Services.LogService.FileLogger
{
	using System;
	using Interfaces;
	using Microsoft.Extensions.Logging;

	public class FileLoggerProvider : ILoggerProvider
	{
		private readonly IFileService _fileService;
		private readonly Func<string, LogLevel, bool> _filter;

		public FileLoggerProvider(Func<string, LogLevel, bool> filter, IFileService fileService)
		{
			_fileService = fileService;
			_filter = filter;
		}

		public void Dispose()
		{
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new FileLoggingService(categoryName, _filter, _fileService);
		}
	}
}