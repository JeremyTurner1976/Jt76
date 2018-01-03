namespace Jt76.Common.Interfaces
{
	using System;
	using Microsoft.Extensions.Logging;

	public interface ILogService
	{
		void LogMessage(string subject, string message);

		void LogError(Exception exception, string message, LogLevel logLevel = LogLevel.None);
	}
}