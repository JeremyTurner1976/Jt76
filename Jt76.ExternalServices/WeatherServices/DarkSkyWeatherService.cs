namespace Jt76.ExternalServices.WeatherServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Common.Extensions;
	using Common.Interfaces;
	using Common.Services.HttpService;
	using ConfigSettings;
	using Enums;
	using Interfaces;
	using Microsoft.Extensions.Logging;
	using Models;
	using Models.DarkSkyModels;

	//https://darksky.net/dev/docs/forecast
	public class DarkSkyWeatherService : IWeatherService
	{
		public enum PrecipitationVolumeTypes
		{
			Rain,
			Snow
		}

		public const int Freezing = 31;

		private readonly ILogger<DarkSkyWeatherService> _logger;

		public DarkSkyWeatherService(
			DarkSkyWeatherServiceSettings weatherServiceSettings,
			ILogger<DarkSkyWeatherService> logger)
		{
			WeatherServiceSettings = weatherServiceSettings;
			_logger = logger;
			;
		}

		public IWeatherServiceSettings WeatherServiceSettings { get; set; }
		public int DayTimeStart { get; set; }
		public int DayTimeEnd { get; set; }

		public async Task<WeatherData> GetWeatherData(double latitude, double longitude)
		{
			DarkSkyWeather darkSkyWeather = await GetWeather(latitude, longitude);
			if (darkSkyWeather != null)
			{
				DayTimeStart = darkSkyWeather.daily.data.FirstOrDefault().sunrise.Hour;
				DayTimeEnd = darkSkyWeather.daily.data.FirstOrDefault().sunset.Hour;

				return new WeatherData
				{
					Description = darkSkyWeather.currently.summary,
					Sunrise = darkSkyWeather.daily.data.FirstOrDefault().sunrise,
					Sunset = darkSkyWeather.daily.data.FirstOrDefault().sunset,
					WeatherForecasts = GetDarkSkyForecasts(darkSkyWeather)
				};
			}

			throw new ArgumentNullException(nameof(darkSkyWeather));
		}

		private async Task<DarkSkyWeather> GetWeather(double latitude, double longitude)
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
				return await response.ParseJsonResponse<DarkSkyWeather>();
			}
		}

		private ICollection<Forecast> GetDarkSkyForecasts(DarkSkyWeather darkSkyWeather)
		{
			List<Forecast> fiveDayOneHourForecasts = (from item in darkSkyWeather.hourly.data
				select new Forecast
				{
					StartDateTime = item.dateTime,
					EndDateTime = item.dateTime.AddHours(1).AddSeconds(-1),
					Description = item.summary,
					Temperature = item.temperature,
					Humidity = item.humidity,
					AtmosphericPressure = item.pressure,
					Windspeed = item.windSpeed,
					WindDirection = item.windBearing,
					SkyCon = item.icon,
					Icon = GetIcon(item.icon, (int) item.cloudCover, item.dateTime.Hour),
					CloudCover = item.cloudCover,
					PrecipitationVolume = item.precipIntensity
				}).ToList();
			int test = fiveDayOneHourForecasts.Count();

			return GetThreeHourSummedForecasts(fiveDayOneHourForecasts);
		}

		private ICollection<Forecast> GetThreeHourSummedForecasts(List<Forecast> fiveDayOneHourForecasts)
		{
			int count = 0;
			int groupCount = 0;
			const int groupSize = 3;
			const int precisionValue = 2;

			Forecast forecast = new Forecast();
			List<Forecast> forecasts = new List<Forecast>();

			double temperatureTotal = 0;
			double humidityTotal = 0;
			double atmosphericPressureTotal = 0;
			double windspeedTotal = 0;
			double windDirectionTotal = 0;
			double cloudCoverTotal = 0;
			double precipitationVolumeTotal = 0;
			double minimumTemperature = 9999;
			double maximumTemperature = -9999;


			foreach (Forecast item in fiveDayOneHourForecasts)
			{
				//Gather averages data
				temperatureTotal += item.Temperature;
				humidityTotal += item.Humidity;
				atmosphericPressureTotal += item.AtmosphericPressure;
				windspeedTotal += item.Windspeed;
				windDirectionTotal += item.WindDirection;
				cloudCoverTotal += item.CloudCover;
				precipitationVolumeTotal += item.PrecipitationVolume;

				//gather extremity data
				minimumTemperature = Math.Min(item.Temperature, minimumTemperature);
				maximumTemperature = Math.Max(item.Temperature, maximumTemperature);

				//handle data for three hour chunks

				if (count % groupSize == 0) //first item handles all resets
				{
					groupCount = 0;

					temperatureTotal = 0;
					humidityTotal = 0;
					atmosphericPressureTotal = 0;
					windspeedTotal = 0;
					windDirectionTotal = 0;
					cloudCoverTotal = 0;
					precipitationVolumeTotal = 0;
					minimumTemperature = 9999;
					maximumTemperature = -9999;
				}
				else if (groupCount == 1) //second item, picks some middle values for a three hour chunk
				{
					forecast = new Forecast
					{
						StartDateTime = item.StartDateTime.AddHours(-1),
						EndDateTime = item.StartDateTime.AddHours(2).AddSeconds(-1),
						Description = item.Description,
						SkyCon = item.SkyCon,
						Icon = item.Icon
					};
				}
				else
				{
					//third item, attaches averages and inserts the item
					forecast.Temperature = (temperatureTotal / groupSize).ToPrecisionValue(precisionValue);
					forecast.Humidity = (humidityTotal / groupSize).ToPrecisionValue(precisionValue);
					forecast.AtmosphericPressure = (atmosphericPressureTotal / groupSize).ToPrecisionValue(precisionValue);
					forecast.Windspeed = (windspeedTotal / groupSize).ToPrecisionValue(precisionValue);
					forecast.WindDirection = (int) (windDirectionTotal / groupSize).ToPrecisionValue(precisionValue);
					forecast.CloudCover = (cloudCoverTotal / groupSize).ToPrecisionValue(precisionValue);
					forecast.PrecipitationVolume = (precipitationVolumeTotal / groupSize).ToPrecisionValue(precisionValue);

					forecast.MinimumTemperature = minimumTemperature;
					forecast.MinimumTemperature = maximumTemperature;

					forecasts.Add(forecast);
				}
				count++;
				groupCount++;
			}

			return forecasts;
		}

		//https://openweathermap.org/weather-conditions
		//https://darkskyapp.github.io/skycons/
		protected virtual string GetIcon(string skyCon, int cloudCover, int hourOfDay)
		{
			switch (skyCon)
			{
				case SkyConClientSideStrings.ClearDay:
					return IconFileNames.ClearSkyDay;
				case SkyConClientSideStrings.ClearNight:
					return IconFileNames.ClearSkyNight;
				case SkyConClientSideStrings.PartlyCloudyDay:
					return IconFileNames.PartlyCloudyDay;
				case SkyConClientSideStrings.PartlyCloudyNight:
					return IconFileNames.PartlyCloudyNight;

				case SkyConClientSideStrings.Cloudy:
					if (cloudCover <= 50)
						return IsDayTime(hourOfDay)
							? IconFileNames.PartlyCloudyDay
							: IconFileNames.PartlyCloudyNight;
					if (cloudCover > 50 && cloudCover < 75)
						return IsDayTime(hourOfDay)
							? IconFileNames.ScatteredCloudsDay
							: IconFileNames.ScatteredCloudsNight;
					return IsDayTime(hourOfDay)
						? IconFileNames.BrokenCloudsDay
						: IconFileNames.BrokenCloudsNight;

				case SkyConClientSideStrings.Rain:
					return IsDayTime(hourOfDay)
						? IconFileNames.RainDay
						: IconFileNames.RainNight;

				case SkyConClientSideStrings.Sleet:
					return IsDayTime(hourOfDay)
						? IconFileNames.ShoweringRainDay
						: IconFileNames.ShoweringRainNight;

				case SkyConClientSideStrings.Snow:
					return IsDayTime(hourOfDay)
						? IconFileNames.SnowDay
						: IconFileNames.SnowNight;

				case SkyConClientSideStrings.Wind:
					return IsDayTime(hourOfDay)
						? IconFileNames.MistDay
						: IconFileNames.MistNight;

				case SkyConClientSideStrings.Fog:
					return IsDayTime(hourOfDay)
						? IconFileNames.PartlyCloudyDay
						: IconFileNames.PartlyCloudyNight;
				default:
					throw new Exception(
						"Not all Code Paths return a value: " +
						skyCon);
			}
		}

		private bool IsDayTime(int hourOfDay)
		{
			return hourOfDay >= DayTimeStart || hourOfDay <= DayTimeEnd;
		}
	}
}