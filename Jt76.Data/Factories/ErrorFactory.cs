namespace Jt76.Data.Factories
{
	using System;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Threading.Tasks;
	using Common.Extensions;
	using Microsoft.Extensions.Logging;
	using Models;

	public static class ErrorFactory
	{

		public static Error GetErrorFromException(Exception e, LogLevel errorLevel, string additionalInformation)
		{
			Error error = new Error
			{
				Message = e.GetBaseException().Message,
				Source = e.GetBaseException().Source,
				ErrorLevel = Enum.GetName(typeof (LogLevel), errorLevel),
				AdditionalInformation = additionalInformation,
				StackTrace = e.StackTrace + Environment.NewLine + (e.InnerException == null
					? "             |No inner exception| "
					: "             |Inner Exception| " + e.InnerException.ToEnhancedString())
		};

			return error;
		}

		public static void ThrowException()
		{
			int n = 0;
			int divideByZero = 1 / n;
		}

		/// <summary>
		/// Throws an aggregate exception.
		/// </summary>
		/// <returns>An awaitable method that will cause an aggregate exception</returns>
		public static Task<string[][]> ThrowAggregateException()
		{
			// Get a folder path whose directories should throw an UnauthorizedAccessException. 
			string path = Directory.GetParent(
				Environment.GetEnvironmentVariable(
					RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home"))
				.FullName;
			;

			Task<string[]> task = Task<string[]>.Factory.StartNew(() => GetAllFiles(path));
			Task<string[]> taskTwo = Task<string[]>.Factory.StartNew(() => { throw new IndexOutOfRangeException(); });

			Task.WaitAll(task, taskTwo); //waits on all
			return Task.WhenAll(task, taskTwo);
		}


		//helper method for throwing an aggregate exception
		private static string[] GetAllFiles(string str)
		{
			// Should throw an UnauthorizedAccessException exception. 
			return Directory.GetFiles(str, "*.txt", SearchOption.AllDirectories);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="additionalInformation"></param>
		/// <param name="logLevel"></param>
		/// <returns></returns>
		public static Error GetInformationalError(
			string message,
			string additionalInformation,
			string logLevel)
		{
			Error error = new Error
			{
				Message = message,
				Source = "Generated Error Message",
				ErrorLevel = logLevel,
				AdditionalInformation = additionalInformation,
				StackTrace = null,
				CreatedDate = DateTime.UtcNow
			};

			return error;
		}
	}
}