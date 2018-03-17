import { IWeatherForecast } from "./weather-forecast";

export interface IWeatherData {
  city: string;
  country: string;
  description: string;
  sunrise: Date;
  sunset: Date;
  weatherForecasts: Array<IWeatherForecast>;
}

export class WeatherData implements IWeatherData {
  city = "";
  country = "";
  description = "";
  sunrise = new Date();
  sunset = new Date();
  weatherForecasts = new Array<IWeatherForecast>();
}
