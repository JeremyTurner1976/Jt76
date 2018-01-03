namespace Jt76.Common.Services.LogService.HttpLogger
{
	using System;
	using System.Diagnostics;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json.Linq;

	//https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/httpclient-message-handlers
	public class HttpClientLogging : DelegatingHandler
	{
		private readonly ILogger _logger;

		public HttpClientLogging(HttpMessageHandler innerHandler, ILogger logger)
			: base(innerHandler)
		{
			_logger = logger;
		}

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request, CancellationToken cancellationToken)
		{
			DateTime startTime = DateTime.UtcNow;

			Stopwatch watch = Stopwatch.StartNew();
			HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
			watch.Stop();

			string logTemplate =
				"Client IP: {clientIP}" +
				"Request path: {requestPath}" +
				"Start time: {startTime}" +
				"Duration: {duration} ms" +
				"Request: {request}" +
				"Response: {response}";

			_logger.LogInformation(
				logTemplate,
				request.RequestUri,
				response.StatusCode,
				startTime,
				watch.ElapsedMilliseconds,
				request,
				JObject.Parse(await response.Content.ReadAsStringAsync()).ToString());

			return response;
		}
	}
}