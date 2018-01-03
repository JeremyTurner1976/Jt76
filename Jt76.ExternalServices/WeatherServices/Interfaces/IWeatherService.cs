namespace Jt76.ExternalServices.WeatherServices.Interfaces
{
	using System;
	using System.Threading.Tasks;
	using Models;

	public interface IWeatherService
	{
		/// <summary>
		///     Gathers weather data for use in any weather api
		///     <para />
		///     This task gathers threaded weather data from 2 webservice calls asynchronously using OpenWeather
		///     <para />
		///     This task gathers weather data asynchronously in a single thread using DarkSkyWeather
		/// </summary>
		/// <param name="latitude">The latitude to gather data for</param>
		/// <param name="longitude">The longitude to gather data for</param>
		/// <returns>A <see cref="WeatherData"> Object from the Open Weather Service Api</see>/></returns>
		/// <exception cref="AggregateException">Parrallel threads are run in this task if using OpenWeather</exception>
		/// <exception cref="ArgumentNullException">If the model is incorrect, Json Parse exceptions will occur</exception>
		/// ///
		/// <exception cref="UriFormatException">
		///     Both the base uri and current/future uri values must be correct in the settings
		///     filesr
		/// </exception>
		Task<WeatherData> GetWeatherData(double latitude, double longitude);
	}
}