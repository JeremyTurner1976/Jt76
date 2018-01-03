namespace Jt76.ExternalServices.WeatherServices.Models
{
	using System;
	using System.Collections.Generic;

	public class WeatherData
	{
		public string Description { get; set; }

		public DateTime Sunrise { get; set; }

		public DateTime Sunset { get; set; }

		public string City { get; set; } = "Unknown";

		public string Country { get; set; } = "USA";

		public ICollection<Forecast> WeatherForecasts { get; set; }
	}
}