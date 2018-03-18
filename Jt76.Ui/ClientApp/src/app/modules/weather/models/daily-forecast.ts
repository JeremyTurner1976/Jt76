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
  temperatureColors: string[];
  windCompassDirection: any;
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
  iconColors = new Array<string>();
  times = new Array<string>();
  temperatures = new Array<string>();
  temperatureColors = new Array<string>();

  windCompassDirection(): string {
    const degrees = this.avgWindDirection;
    let windDirection = "";

    if (degrees > 337.5) {
      windDirection += "North";
    } else if (degrees > 292.5) {
      windDirection += "NorthWest";
    } else if (degrees > 247.5) {
      windDirection += "West";
    } else if (degrees > 202.5) {
      windDirection += "SouthWest";
    } else if (degrees > 157.5) {
      windDirection += "South";
    } else if (degrees > 122.5) {
      windDirection += "SouthEast";
    } else if (degrees > 67.5) {
      windDirection += "East";
    } else if (degrees > 22.5) {
      windDirection += "NorthEast";
    } else {
      windDirection += "North";
    }

    return windDirection;
  }
}
