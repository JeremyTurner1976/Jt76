namespace Jt76.WebApi.Controllers
{
	using System.Threading.Tasks;
	using ExternalServices.WeatherServices;
	using ExternalServices.WeatherServices.Models;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/v1/[controller]")]
	public class WeatherDataController : Controller
	{
		public WeatherDataController(
			DarkSkyWeatherService darkSkyWeatherService,
			OpenWeatherService openWeatherService)
		{
			DarkSkyWeatherService = darkSkyWeatherService;
			OpenWeatherService = openWeatherService;
		}

		public DarkSkyWeatherService DarkSkyWeatherService { get; set; }
		public OpenWeatherService OpenWeatherService { get; set; }

		/*
		 * TODO
			//Implement weather service and GeoLocation
			//very detailed single day displays with graphs and such (will be a router, with a defined route just like errors)

			//Paging on Errors
			//Decorate the error page
			//Implement sass variables - alert will use these
			//Implement global event bus (For theme) - can use observe on local storage, themed alert. Should look into traditional methods

			//Implement Authentication - https://social.technet.microsoft.com/wiki/contents/articles/37169.secure-your-netcore-web-applications-using-identityserver-4.aspx
			//implement user admin
			//Implement User Secrets Manager
			//storage cleanup service (Use injectors to get all services, check cache time and delete over on app start)

			//Implement weather service and GeoLocation
			//Finalized display for open weather api forecasts

			//Implement dashboards
			//Clean up references

			//Implement EntityAttributeFramework
			//Implement Pdf and excel outputs

			//Implement tests
			//refactor and add comments

			//Implement prod deploy settings and test
		 */

		[HttpGet("DarkSkyWeatherForecasts")]
		[ProducesResponseType(typeof(WeatherData), 200)]
		public async Task<IActionResult> DarkSkyWeatherForecasts()
		{
			WeatherData weatherForecast = await DarkSkyWeatherService.GetWeatherData(1, 2);
			return Ok(weatherForecast);
		}

		[HttpGet("OpenWeatherForecasts")]
		[ProducesResponseType(typeof(WeatherData), 200)]
		public async Task<IActionResult> OpenWeatherForecasts()
		{
			WeatherData weatherForecast = await OpenWeatherService.GetWeatherData(1, 2);
			return Ok(weatherForecast);
		}
	}
}