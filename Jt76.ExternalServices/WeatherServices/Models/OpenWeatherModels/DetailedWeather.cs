namespace Jt76.ExternalServices.WeatherServices.Models.OpenWeatherModels
{
	//http://json2csharp.com/
	//https://openweathermap.org/current
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class DetailedWeather
	{
		public List<Weather> weather { get; set; }

		public Main main { get; set; }

		public int visibility { get; set; }

		public Wind wind { get; set; }

		public Clouds clouds { get; set; }

		[JsonProperty(PropertyName = "sys")]
		public Sys systemInformation { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string city { get; set; }

		[JsonProperty(PropertyName = "rain")]
		public Rain rainTotal { get; set; }

		[JsonProperty(PropertyName = "snow")]
		public Snow snowTotal { get; set; }
	}
}