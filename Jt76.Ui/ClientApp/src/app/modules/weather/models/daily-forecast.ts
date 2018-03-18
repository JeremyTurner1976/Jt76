export interface IDailyForecast {
  day: string;
  avgAtmosphericPressure: number;
  avgCloudCover: number;
  avgHumidity: number;
  maximumTemperature: number;
  minimumTemperature: number;
  totalPrecipitationVolume: number;
  avgTemperature: number;
  avgWindDirection: number;
  minWindspeed: number;
  maxWindspeed: number;
  descriptions: string[];
  skyCons: string[];
  times: string[];
  temperatures: string[];
}

export class DailyForecast implements IDailyForecast {
  day = "";
  avgAtmosphericPressure = 0;
  avgCloudCover = 0;
  avgHumidity = 0;
  maximumTemperature = -9999;
  minimumTemperature = 9999;
  totalPrecipitationVolume = 0;
  avgTemperature = 0;
  avgWindDirection = 0;
  maxWindspeed = -9999;
  minWindspeed = 9999;
  descriptions = new Array<string>();
  skyCons = new Array<string>();
  times = new Array<string>();
  temperatures = new Array<string>();
}
