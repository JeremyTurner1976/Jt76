namespace Jt76.ExternalServices.WeatherServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Common.Attributes;
	using Common.Extensions;
	using Common.Interfaces;
	using Common.Services.HttpService;
	using ConfigSettings;
	using Enums;
	using Interfaces;
	using Microsoft.Extensions.Logging;
	using Models;
	using Models.OpenWeatherModels;

	//https://openweathermap.org/api
	public class OpenWeatherService : IWeatherService
	{
		private readonly ILogger<OpenWeatherService> _logger;

		public OpenWeatherService(
			OpenWeatherServiceSettings weatherServiceSettings,
			ILogger<OpenWeatherService> logger)
		{
			WeatherServiceSettings = weatherServiceSettings;
			_logger = logger;
		}

		public IWeatherServiceSettings WeatherServiceSettings { get; set; }

		public async Task<WeatherData> GetWeatherData(double latitude, double longitude)
		{
			Task<DetailedWeather> asyncCurrentWeather = GetCurrentWeather(latitude, longitude);
			Task<ICollection<Forecast>> asyncFutureWeather = GetFutureWeather(latitude, longitude);

			await Task.WhenAll(asyncCurrentWeather, asyncFutureWeather);
			DetailedWeather currentWeather = asyncCurrentWeather.Result;
			Weather weather = currentWeather?.weather?.FirstOrDefault();
			ICollection<Forecast> forecasts = asyncFutureWeather.Result;

			if (currentWeather != null && weather != null && forecasts != null)
				return new WeatherData
				{
					Description = weather.description,
					Sunrise = currentWeather.systemInformation.sunriseTime,
					Sunset = currentWeather.systemInformation.sunsetTime,
					City = currentWeather.city,
					Country = currentWeather.systemInformation.country,
					WeatherForecasts = forecasts
				};

			if (currentWeather == null)
				throw new ArgumentNullException(nameof(currentWeather));
			if (weather == null)
				throw new ArgumentNullException(nameof(weather));

			throw new ArgumentNullException(nameof(forecasts));
		}

		private async Task<DetailedWeather> GetCurrentWeather(double latitude, double longitude)
		{
			Uri clientUri = new Uri(
				WeatherServiceSettings.BaseUri,
				WeatherServiceSettings.CurrentWeatherRelativeUri);

			using (IWebService webService = new RestService(_logger))
			{
				HttpRequestMessage httpRequest =
					new HttpRequestMessage(HttpMethod.Get, clientUri);
				HttpResponseMessage response = await webService.SendAsync(httpRequest);
				response.EnsureSuccessStatusCode();
				return await response.ParseJsonResponse<DetailedWeather>();
			}
		}

		private async Task<ICollection<Forecast>> GetFutureWeather(double latitude, double longitude)
		{
			Uri clientUri = new Uri(
				WeatherServiceSettings.BaseUri,
				WeatherServiceSettings.FutureWeatherRelativeUri);

			using (IWebService webService = new RestService(_logger))
			{
				HttpRequestMessage httpRequest =
					new HttpRequestMessage(HttpMethod.Get, clientUri);
				HttpResponseMessage response = await webService.SendAsync(httpRequest);
				response.EnsureSuccessStatusCode();
				ThreeHourFiveDayForecast threeHourFiveDayForecast =
					await response.ParseJsonResponse<ThreeHourFiveDayForecast>();

				return (from item in threeHourFiveDayForecast.forecasts
					let weather = item.weather.FirstOrDefault()
					select new Forecast
					{
						StartDateTime = item.startDateTime,
						EndDateTime = item.startDateTime.AddHours(3).AddSeconds(-1),
						Description = weather.description,
						Temperature = item.main.temperature,
						MinimumTemperature = item.main.minimumTemperature,
						MaximumTemperature = item.main.maximumTemperature,
						Humidity = item.main.humidity,
						AtmosphericPressure = item.main.pressure,
						Windspeed = item.wind.speed,
						WindDirection = (int) item.wind.degrees,
						SkyCon = GetSkyCon(weather.icon),
						Icon = weather.icon,
						CloudCover = item.clouds.cloudCover,
						PrecipitationVolume =
							item.rainTotal?.threeHourTotal ?? 0
							+ item.snowTotal?.threeHourTotal ?? 0
					}).ToList();
			}
		}

		//https://openweathermap.org/weather-conditions
		//https://darkskyapp.github.io/skycons/

		protected virtual string GetSkyCon(string icon)
		{
			switch (icon)
			{
				case IconFileNames.ClearSkyDay:
					return SkyCons.ClearDay.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.ClearSkyNight:
					return SkyCons.ClearNight.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.PartlyCloudyDay:
				case IconFileNames.BrokenCloudsDay:
					return SkyCons.PartlyCloudyDay.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.PartlyCloudyNight:
				case IconFileNames.BrokenCloudsNight:
					return SkyCons.PartlyCloudyNight.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.ScatteredCloudsDay:
				case IconFileNames.ScatteredCloudsNight:
					return SkyCons.Cloudy.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.RainDay:
				case IconFileNames.RainNight:
					return SkyCons.Rain.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.ShoweringRainDay:
				case IconFileNames.ShoweringRainNight:
					return SkyCons.Sleet.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.SnowDay:
				case IconFileNames.SnowNight:
					return SkyCons.Snow.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.ThunderStormDay:
				case IconFileNames.ThunderStormNight:
					return SkyCons.Wind.ToAttributeString<EnumAttributes.ClientSideString>();
				case IconFileNames.MistDay:
				case IconFileNames.MistNight:
					return SkyCons.Fog.ToAttributeString<EnumAttributes.ClientSideString>();
				default:
					throw new Exception(
						"Not all Code Paths return a value: " +
						icon ?? "Null Icon");
			}
		}
	}
}