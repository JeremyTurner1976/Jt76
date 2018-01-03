namespace Jt76.Common.Services.LogService.EmailLogger
{
	using System;
	using Abstract;
	using ConfigSettings.EmailSettings;
	using Interfaces;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;

	public class EmailLoggingService : BaseLogger
	{
		private readonly EmailSettings _emailSettings;
		private readonly IEmailService _mailService;

		public EmailLoggingService(string categoryName, Func<string, LogLevel, bool> filter, IEmailService mailService,
			IOptions<EmailSettings> emailSettings)
		{
			CategoryName = categoryName;
			Filter = filter;
			_mailService = mailService;
			_emailSettings = emailSettings.Value;
		}


		public override void Log(LogLevel logLevel, int eventId, object state, Exception exception,
			Func<object, Exception, string> formatter)
		{
			string message = VerifyAndGenerateMessage(logLevel, state, exception, formatter);

			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			string subject = GetSubject(logLevel);
			_mailService.SendMail(_emailSettings.DeveloperEmailAddress, _emailSettings.CarbobCopyEmailAddress,
				_emailSettings.BackupCarbonCopyEmailAddress, subject, message);
		}

		public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
			Func<TState, Exception, string> formatter)
		{
			string message = VerifyAndGenerateMessage(logLevel, state, exception, formatter);

			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			string subject = GetSubject(logLevel);
			_mailService.SendMail(_emailSettings.DeveloperEmailAddress, _emailSettings.CarbobCopyEmailAddress,
				_emailSettings.BackupCarbonCopyEmailAddress, subject, message);
		}

		public override void LogError(
			Exception exception,
			string message,
			LogLevel logLevel = LogLevel.None)
		{
			_mailService.SendMail(_emailSettings.DeveloperEmailAddress, _emailSettings.CarbobCopyEmailAddress,
				_emailSettings.BackupCarbonCopyEmailAddress, errorSubject, message);
		}

		public override void LogMessage(string subject, string message)
		{
			_mailService.SendMail(_emailSettings.DeveloperEmailAddress, _emailSettings.CarbobCopyEmailAddress,
				_emailSettings.BackupCarbonCopyEmailAddress, subject, message);
		}
	}
}