namespace Jt76.ExternalServices.WeatherServices.Models.OpenWeatherModels
{
	//http://json2csharp.com/
	//https://openweathermap.org/forecast5
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class ThreeHourFiveDayForecast
	{
		public double message { get; set; }

		[JsonProperty(PropertyName = "cnt")]
		public int listCount { get; set; }

		[JsonProperty(PropertyName = "list")]
		public List<List> forecasts { get; set; }
	}
}