namespace Jt76.ExternalServices.WeatherServices.Models
{
	using System;

	public class Forecast
	{
		public DateTime StartDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public string Description { get; set; }

		public double Temperature { get; set; }

		public double MinimumTemperature { get; set; }

		public double MaximumTemperature { get; set; }

		public double Humidity { get; set; }

		public double AtmosphericPressure { get; set; }

		public double Windspeed { get; set; }

		public int WindDirection { get; set; }

		public string SkyCon { get; set; }

		public string Icon { get; set; }

		public double CloudCover { get; set; }

		public double PrecipitationVolume { get; set; }
	}
}