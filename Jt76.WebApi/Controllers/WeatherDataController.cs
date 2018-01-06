namespace Jt76.WebApi.Controllers
{
	using System.Threading.Tasks;
	using ExternalServices.WeatherServices;
	using ExternalServices.WeatherServices.Models;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/v1/[controller]")]
	public class WeatherDataController : Controller
	{
		public DarkSkyWeatherService DarkSkyWeatherService { get; set;  }
		public OpenWeatherService OpenWeatherService { get; set;  }

		public WeatherDataController(
			DarkSkyWeatherService darkSkyWeatherService, 
			OpenWeatherService openWeatherService)
		{
			DarkSkyWeatherService = darkSkyWeatherService;
			OpenWeatherService = openWeatherService;
		}

		/*
			//For testing database is working
			//int usersCount = userRepository.Count();
			//int errorsCount = errorRepository.Count();

			//For testing error loggers
			//ErrorFactory.ThrowException();

			//Client Side work left and geolocation left

			//Implement a file getter page for developers (add webservice file Directory, output as html), and database errors
			//Implement weather service
			//Implement Client Side Errors
			//Implement toaster - Perhaps production error handler output there, but create much smaller html to display in the toaster (Message)

			//Implement Authentication - https://social.technet.microsoft.com/wiki/contents/articles/37169.secure-your-netcore-web-applications-using-identityserver-4.aspx
			//Implement User Secrets Manager
		 */

		[HttpGet("DarkSkyWeatherForecasts")]
		[ProducesResponseType(typeof (WeatherData), 200)]
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
 