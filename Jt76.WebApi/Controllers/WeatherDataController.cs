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
			//Paging on Errors
			//Implement toaster - Perhaps production error handler output there, but create much smaller html to display in the toaster (Message)
			//Implement loading screen and load guards like in the other app
			//Implement sass variables
			//Implement local storage for settings
			//Implement clean services and data store

			//Implement Authentication - https://social.technet.microsoft.com/wiki/contents/articles/37169.secure-your-netcore-web-applications-using-identityserver-4.aspx
			//implement user admin
			//Implement User Secrets Manager

			//Implement weather service and GeoLocation

			//Implement dashboards
			//Implement phone layout
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