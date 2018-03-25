import { KeyValue }
  from "../../../shared/models/key-value";

export interface IWeatherForecast {
  atmosphericPressure: number;
  cloudCover: number;
  description: string;
  endDateTime: Date;
  humidity: number;
  icon: string;
  maximumTemperature: number;
  minimumTemperature: number;
  precipitationVolume: number;
  skyCon: string;
  startDateTime: Date;
  temperature: number;
  windDirection: number;
  windspeed: number;
  detailModel: Array<KeyValue>;
}

export class WeatherForecast implements IWeatherForecast {
  atmosphericPressure = 0;
  cloudCover = 0;
  description = "";
  endDateTime = new Date();
  humidity = 0;
  icon = "";
  maximumTemperature = 0;
  minimumTemperature = 0;
  precipitationVolume = 0; 
  skyCon = "";
  startDateTime = new Date();
  temperature = 0;
  windDirection = 0;
  windspeed = 0;
  detailModel = new Array<KeyValue>();
}
