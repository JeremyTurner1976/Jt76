namespace Jt76.Common.Extensions
{
	using System.Net.Http;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Newtonsoft.Json;
	using Services.HttpService.Models;

	public static class HttpExtensions
	{
		public static async Task<T> ParseJsonResponse<T>(this HttpResponseMessage response)
			where T : class
		{
			string stringResult = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(stringResult);
		}

		public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems,
			int totalPages)
		{
			response.Headers.Add("Pagination",
				JsonConvert.SerializeObject(new PageHeader(currentPage, itemsPerPage, totalItems, totalPages)));
			response.Headers.Add("access-control-expose-headers", "Pagination"); // CORS
		}

		public static void AddApplicationError(this HttpResponse response, string message)
		{
			response.Headers.Add("Application-Error", message);
			response.Headers.Add("access-control-expose-headers", "Application-Error"); // CORS
		}
	}
}