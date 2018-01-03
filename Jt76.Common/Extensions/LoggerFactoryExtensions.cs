namespace Jt76.Common.Extensions
{
	using System;
	using ConfigSettings.EmailSettings;
	using Interfaces;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Services.LogService.EmailLogger;
	using Services.LogService.FileLogger;

	public static class LoggerFactoryExtensions
	{
		public static ILoggerFactory AddEmailLogger(
			this ILoggerFactory factory,
			IEmailService mailService,
			IOptions<EmailSettings> emailSettings,
			Func<string, LogLevel, bool> filter = null)
		{
			factory.AddProvider(new EmailLoggerProvider(filter, mailService, emailSettings));
			return factory;
		}

		public static ILoggerFactory AddEmailLogger(
			this ILoggerFactory factory,
			IEmailService mailService,
			IOptions<EmailSettings> emailSettings,
			LogLevel minLevel)
		{
			return AddEmailLogger(
				factory,
				mailService,
				emailSettings,
				(_, logLevel) => logLevel >= minLevel);
		}

		public static ILoggerFactory AddFileLogger(
			this ILoggerFactory factory,
			IFileService fileservice,
			Func<string, LogLevel, bool> filter = null)
		{
			factory.AddProvider(new FileLoggerProvider(filter, fileservice));
			return factory;
		}

		public static ILoggerFactory AddFileLogger(
			this ILoggerFactory factory,
			IFileService fileservice,
			LogLevel minLevel)
		{
			return AddFileLogger(
				factory,
				fileservice,
				(_, logLevel) => logLevel >= minLevel);
		}
	}
}