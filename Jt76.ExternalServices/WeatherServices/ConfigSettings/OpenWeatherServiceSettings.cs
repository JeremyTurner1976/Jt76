namespace Jt76.ExternalServices.WeatherServices.ConfigSettings
{
	using System;
	using Common.Extensions;
	using Interfaces;

	public class OpenWeatherServiceSettings : IWeatherServiceSettings
	{
		public enum UnitsOfMeasurement
		{
			imperial,
			metric
		}

		//https://openweathermap.org/current
		//https://openweathermap.org/forecast5

		//TODO Remove this when I have Latitude/Longitude
		public string City { get; } = "Bigfork,Mt";

		private string AppId { get; } = "cc9bb5f7256330c1e1a0f5ba66ccadcd";
		private string CurrentWeatherUri { get; } = "/weather";
		private string FutureWeatherUri { get; } = "/forecast";
		public UnitsOfMeasurement UnitOfMeasurement { get; set; } = UnitsOfMeasurement.imperial;

		public double? Latitude { get; set; } = 47.9960612;
		public double? Longitude { get; set; } = -114.0517769;

		public string ApiRoot { get; } = "/data/";
		public double? Version { get; set; } = 2.5;

		public Uri BaseUri { get; } = new Uri("http://api.openweathermap.org");

		public string CurrentWeatherRelativeUri
			=> ApiRoot +
			   $"{Version}" +
			   CurrentWeatherUri +
			   "?lat=" + Latitude +
			   "&lon=" + Longitude +
			   "&appid=" + AppId +
			   "&units=" + UnitOfMeasurement.ToNameString();

		public string FutureWeatherRelativeUri
			=> ApiRoot +
			   $"{Version}" +
			   FutureWeatherUri +
			   "?lat=" + Latitude +
			   "&lon=" + Longitude +
			   "&appid=" + AppId +
			   "&units=" + UnitOfMeasurement.ToNameString();
	}
}