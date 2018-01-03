namespace Jt76.Common.Abstract
{
	using System;
	using Extensions;
	using Interfaces;
	using Microsoft.Extensions.Logging;

	public abstract class BaseLogger : ILogger, ILogService
	{
		protected const string messageSubject = "[Angular2CoreBase Log]";
		protected const string errorSubject = "[Angular2CoreBase Error]";

		protected string CategoryName { get; set; }
		protected Func<string, LogLevel, bool> Filter { get; set; }
		public abstract void LogMessage(string subject, string message);
		public abstract void LogError(Exception exception, string message, LogLevel logLevel = LogLevel.None);

		public bool IsEnabled(LogLevel logLevel)
		{
			return Filter == null || Filter(CategoryName, logLevel);
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			// Not necessary
			return null;
		}

		public abstract void Log(LogLevel logLevel, int eventId, object state, Exception exception,
			Func<object, Exception, string> formatter);

		public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
			Func<TState, Exception, string> formatter);

		protected string VerifyAndGenerateMessage(
			LogLevel logLevel,
			object state,
			Exception exception,
			Func<object, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return null;
			}

			if (formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}

			string message = formatter(state, exception);

			if (string.IsNullOrWhiteSpace(message))
			{
				return null;
			}

			message = $"{logLevel}: {message}";

			if (exception != null)
			{
				message += Environment.NewLine + Environment.NewLine + exception.ToEnhancedString();
			}

			return message;
		}

		protected string VerifyAndGenerateMessage<TState>(
			LogLevel logLevel,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return null;
			}

			if (formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}

			string message = formatter(state, exception);

			if (string.IsNullOrWhiteSpace(message))
			{
				return null;
			}

			message = $"{logLevel}: {message}";

			if (exception != null)
			{
				message += Environment.NewLine + Environment.NewLine + exception.ToEnhancedString();
			}

			return message;
		}

		public string GetSubject(LogLevel logLevel)
			=> logLevel < LogLevel.Error
				? messageSubject
				: errorSubject;
	}
}