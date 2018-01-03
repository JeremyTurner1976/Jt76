namespace Jt76.ExternalServices.WeatherServices.Models.DarkSkyModels
{
	using System;
	using System.Collections.Generic;
	using Common.Extensions;

	//http://json2csharp.com/
	//https://darksky.net/dev/docs/forecast

	public class DarkSkyWeather
	{
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string timezone { get; set; }
		public int offset { get; set; }
		public Currently currently { get; set; }
		public Minutely minutely { get; set; }
		public Hourly hourly { get; set; }
		public Daily daily { get; set; }

		public string units { get; set; }

		public class Currently
		{
			public long time { get; set; }

			public DateTime dateTime
				=> time.GetDateTimeFromUnixTimestamp();

			public string summary { get; set; }
			public string icon { get; set; }
			public double nearestStormDistance { get; set; }
			public double nearestStormBearing { get; set; }
			public double precipIntensity { get; set; }
			public double precipProbability { get; set; }
			public double temperature { get; set; }
			public double apparentTemperature { get; set; }
			public double dewPoint { get; set; }
			public double humidity { get; set; }
			public double windSpeed { get; set; }
			public int windBearing { get; set; }
			public double visibility { get; set; }
			public double cloudCover { get; set; }
			public double pressure { get; set; }
			public double ozone { get; set; }
		}

		public class Datum
		{
			public long time { get; set; }

			public DateTime dateTime
				=> time.GetDateTimeFromUnixTimestamp();

			public double precipIntensity { get; set; }
			public double precipProbability { get; set; }
		}

		public class Minutely
		{
			public string summary { get; set; }
			public string icon { get; set; }
			public List<Datum> data { get; set; }
		}

		public class Datum2
		{
			public long time { get; set; }

			public DateTime dateTime
				=> time.GetDateTimeFromUnixTimestamp();

			public string summary { get; set; }
			public string icon { get; set; }
			public double precipIntensity { get; set; }
			public double precipProbability { get; set; }
			public double temperature { get; set; }
			public double apparentTemperature { get; set; }
			public double dewPoint { get; set; }
			public double humidity { get; set; }
			public double windSpeed { get; set; }
			public int windBearing { get; set; }
			public double visibility { get; set; }
			public double cloudCover { get; set; }
			public double pressure { get; set; }
			public double ozone { get; set; }
		}

		public class Hourly
		{
			public string summary { get; set; }
			public string icon { get; set; }
			public List<Datum2> data { get; set; }
		}

		public class Datum3
		{
			public long time { get; set; }

			public DateTime dateTime
				=> time.GetDateTimeFromUnixTimestamp();

			public string summary { get; set; }
			public string icon { get; set; }
			public long sunriseTime { get; set; }
			public long sunsetTime { get; set; }

			public DateTime sunrise
				=> sunriseTime.GetDateTimeFromUnixTimestamp();

			public DateTime sunset
				=> sunsetTime.GetDateTimeFromUnixTimestamp();

			public double moonPhase { get; set; }
			public double precipIntensity { get; set; }
			public double precipIntensityMax { get; set; }
			public double precipProbability { get; set; }
			public double temperatureMin { get; set; }
			public long temperatureMinTime { get; set; }
			public double temperatureMax { get; set; }
			public long temperatureMaxTime { get; set; }
			public double apparentTemperatureMin { get; set; }
			public long apparentTemperatureMinTime { get; set; }
			public double apparentTemperatureMax { get; set; }
			public long apparentTemperatureMaxTime { get; set; }
			public double dewPoint { get; set; }
			public double humidity { get; set; }
			public double windSpeed { get; set; }
			public int windBearing { get; set; }
			public double visibility { get; set; }
			public double cloudCover { get; set; }
			public double pressure { get; set; }
			public double ozone { get; set; }
			public long? precipIntensityMaxTime { get; set; }
			public string precipType { get; set; }
			public double? precipAccumulation { get; set; }
		}

		public class Daily
		{
			public string summary { get; set; }
			public string icon { get; set; }
			public List<Datum3> data { get; set; }
		}
	}
}