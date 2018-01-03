namespace Jt76.ExternalServices.WeatherServices.Interfaces
{
	using Common.Interfaces;

	public interface IWeatherServiceSettings : IWebServiceSettings
	{
		double? Latitude { get; set; }
		double? Longitude { get; set; }

		string CurrentWeatherRelativeUri { get; }
		string FutureWeatherRelativeUri { get; }
	}
}