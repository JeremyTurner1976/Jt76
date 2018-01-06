namespace Jt76.Data.Services.LoggingServices
{
	using System;
	using Common.Abstract;
	using Common.CommonData.Interfaces;
	using Common.Enums;
	using Common.Extensions;
	using Factories;
	using Microsoft.Extensions.Logging;
	using Models;

	public class DatabaseLoggingService : BaseLogger, IDatabaseLoggingService
	{
		private readonly IServiceProvider _serviceProvider;

		public DatabaseLoggingService(
			string categoryName, 
			Func<string, LogLevel, bool> filter,
			IServiceProvider serviceProvider)
		{
			CategoryName = categoryName;
			Filter = filter;
			_serviceProvider = serviceProvider;
		}

		public override void LogError(
			Exception exception,
			string message,
			LogLevel logLevel = LogLevel.None)
		{
			LogErrorAndSave(exception, message, logLevel);
		}

		public override void LogMessage(string subject, string message)
		{
			Error error = ErrorFactory.GetInformationalError(subject, message, LogLevel.Information.ToNameString());
			
			//Must use ServiceProvider as DBContext is Scope Specific and Thrown Errors dispose of this context
			var unitOfWork = _serviceProvider.GetService(typeof(UnitOfWork)) as UnitOfWork;
			unitOfWork.Errors.Add(error);
			unitOfWork.SaveChanges();
		}


		public override void Log(LogLevel logLevel, int eventId, object state, Exception exception,
			Func<object, Exception, string> formatter)
		{
			string message = VerifyAndGenerateMessage(logLevel, state, exception, formatter);

			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			LogErrorAndSave(exception, message, logLevel);
		}

		public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
			Func<TState, Exception, string> formatter)
		{
			string message = VerifyAndGenerateMessage(logLevel, state, exception, formatter);

			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			LogErrorAndSave(exception, message, logLevel);
		}

		private void LogErrorAndSave(
			Exception exception,
			string message,
			LogLevel logLevel = LogLevel.None)
		{
			Error error = ErrorFactory.GetErrorFromException(exception, logLevel, logLevel.ToNameString()
				+ ": " + exception.GetBaseException().Message);

			//TODO handle application users and id, for now just logging to the System Admin 
			//error = _errorDecorator.GetDecoratedModel(error, 1);

			//Must use ServiceProvider as DBContext is Scope Specific and Thrown Errors dispose of this context
			var unitOfWork = _serviceProvider.GetService(typeof(UnitOfWork)) as UnitOfWork;
			unitOfWork.Errors.Add(error);
			unitOfWork.SaveChanges();
		}

		public DirectoryFolders GetFolder(LogLevel logLevel)
			=> logLevel <= LogLevel.Error
				? DirectoryFolders.Logs
				: DirectoryFolders.Errors;
	}
}