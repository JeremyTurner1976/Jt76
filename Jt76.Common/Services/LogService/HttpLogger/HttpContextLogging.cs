namespace Jt76.Common.Services.LogService.HttpLogger
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;

	public class HttpContextLogging
	{
		private readonly ILogger<HttpContextLogging> _logger;
		private readonly RequestDelegate _next;

		public HttpContextLogging(RequestDelegate next, ILogger<HttpContextLogging> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			DateTime startTime = DateTime.UtcNow;

			Stopwatch watch = Stopwatch.StartNew();
			await _next.Invoke(context);
			watch.Stop();

			string logTemplate = @"
				Client IP: {clientIP}
				Request path: {requestPath}
				Start time: {startTime}
				Duration: {duration} ms
				Request: {request}
				Response: {response}";

			_logger.LogInformation(
				logTemplate,
				context.Connection.RemoteIpAddress.ToString(),
				context.Request.Path,
				startTime,
				watch.ElapsedMilliseconds,
				context.Request.Body,
				context.Response.Body);
		}
	}
}