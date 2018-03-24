import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AlertService }
  from "../../../shared/services/alert.service";
import { AppLocalStorageService }
  from "../../../shared/services/app-local-storage.service";
import { BaseService }
  from "../../../shared/abstract/base.service";
import { IWeatherData }
  from "../models/weather-data";
import { DailyForecast }
  from "../models/daily-forecast";
import { WeatherForecast }
  from "../models/weather-forecast";
import * as moment from "moment";

@Injectable()
export class WeatherService extends BaseService<IWeatherData> {
  dayFormat = "dddd";

  constructor(
    public http: HttpClient,
    public alertService: AlertService,
    public appLocalStorageService: AppLocalStorageService) {

    super(http, alertService, appLocalStorageService);
    this.singularName = "WeatherForecast";
    this.dataSetKey = "WeatherForecasts";
    //this.dataUrl = "v1/weatherData/openWeatherForecasts";
    this.dataUrl = "v1/weatherData/darkSkyWeatherForecasts";
    this.dataCacheDuration = 30;
  }

  setUrl(useOpenWeather: boolean) {
    this.dataUrl =
      useOpenWeather
        ? "v1/weatherData/openWeatherForecasts"
        : "v1/weatherData/darkSkyWeatherForecasts";
  }

  getForecastsForDay(forecasts: Array<WeatherForecast>,
    day: string): Array<WeatherForecast> {

    return this.getMatchingForecasts(forecasts, day, true);
  }

  getMatchingForecasts(
    forecasts: Array<WeatherForecast>,
    day: string,
    getOverrunData: boolean): Array<WeatherForecast> {

    return forecasts.filter(
      (item) => {
        if (this.isLimitedByDay(item.startDateTime)) {
          if (getOverrunData) {
            return moment(item.startDateTime)
              .subtract(1, "m").format(this.dayFormat) === day;
          }
          return moment(item.startDateTime).format(this.dayFormat) === day;
        }
        return false;
      });
  }

  getDailyForecasts(forecasts: Array<WeatherForecast>)
    : Array<DailyForecast> {
    
    const days = new Array<string>();
    for (let i = 0; i < forecasts.length; i++) {
      const day = moment(forecasts[i].startDateTime).format("dddd");
      if (!days.includes(day)) {
        days.push(day);
      }
    }

    const dailyForecasts = new Array<DailyForecast>();
    days.forEach(
      (day) => {
        const matchingForecasts =
          this.getMatchingForecasts(forecasts, day, false);

        var dailyForecastCount = matchingForecasts.length;
        
        if (dailyForecastCount) {
          const dailyForecast =
            this.summarizeMatchingForecasts(matchingForecasts);

          dailyForecast.day = day;
          dailyForecast.totalPrecipitationVolume =
            +(dailyForecast.totalPrecipitationVolume).toFixed(2);
          dailyForecast.minimumTemperature =
            +(dailyForecast.minimumTemperature).toFixed(0);
          dailyForecast.maximumTemperature =
            +(dailyForecast.maximumTemperature).toFixed(0);
          dailyForecast.minWindspeed =
            +(dailyForecast.minWindspeed).toFixed(0);
          dailyForecast.maxWindspeed =
            +(dailyForecast.maxWindspeed).toFixed(0);

          dailyForecast.avgAtmosphericPressure =
            +(dailyForecast.avgAtmosphericPressure / dailyForecastCount).toFixed(2);
          dailyForecast.avgCloudCover =
            +(dailyForecast.avgCloudCover / dailyForecastCount).toFixed(2);
          dailyForecast.avgHumidity =
            +(dailyForecast.avgHumidity / dailyForecastCount).toFixed(2);
          dailyForecast.avgTemperature =
            +(dailyForecast.avgTemperature / dailyForecastCount).toFixed(2);
          dailyForecast.avgWindDirection =
            +(dailyForecast.avgWindDirection / dailyForecastCount).toFixed(0);

          dailyForecasts.push(dailyForecast);
        }
      });

    return dailyForecasts;
  }

  isUsingOpenWeather() {
    return this.dataUrl.indexOf("openWeather") >= 0;
  }

  private summarizeMatchingForecasts(
    matchingForecasts: Array<WeatherForecast>)
    : DailyForecast {

    const dailyForecast = new DailyForecast();

    matchingForecasts.forEach(
      (forecast) => {
        dailyForecast.totalPrecipitationVolume += forecast.precipitationVolume;

        dailyForecast.avgTemperature += forecast.temperature;
        dailyForecast.avgWindDirection += forecast.windDirection;
        dailyForecast.avgAtmosphericPressure += forecast.atmosphericPressure;
        dailyForecast.avgCloudCover += forecast.cloudCover;
        dailyForecast.avgHumidity += forecast.humidity;

        dailyForecast.maximumTemperature =
          dailyForecast.maximumTemperature < forecast.maximumTemperature
          ? forecast.maximumTemperature
          : dailyForecast.maximumTemperature;
        dailyForecast.minimumTemperature =
          forecast.minimumTemperature < dailyForecast.minimumTemperature
          ? forecast.minimumTemperature
          : dailyForecast.minimumTemperature;
        dailyForecast.maxWindspeed =
          dailyForecast.maxWindspeed < forecast.windspeed
          ? forecast.windspeed
          : dailyForecast.maxWindspeed;
        dailyForecast.minWindspeed =
          forecast.windspeed < dailyForecast.minWindspeed
          ? forecast.windspeed
          : dailyForecast.minWindspeed;

        const temperature =
          ((forecast.minimumTemperature + forecast.maximumTemperature) / 2);
        dailyForecast.descriptions.push(
          forecast.description);
        dailyForecast.skyCons.push(
          forecast.skyCon);
        dailyForecast.times.push(
          moment(forecast.startDateTime).format("h:mm a"));
        dailyForecast.temperatures.push(
          temperature.toFixed(0));
        dailyForecast.temperatureColors.push(
          this.getTemperatureColor(temperature));
      });

    return dailyForecast;
  };

  private isLimitedByDay(forecastStartDate: Date) {
    const now = moment();
    const startDateTime = moment(forecastStartDate);
    return now.format(this.dayFormat) !== startDateTime.format(this.dayFormat)
      || (
        now.format(this.dayFormat) === startDateTime.format(this.dayFormat) &&
        now.date() === startDateTime.date()
      );
  }

  private getTemperatureColor(temperature: number): string {
    if (temperature > 100) {
      return "#E65100";
    } else if (temperature > 95) {
      return "#EF6C00";
    } else if (temperature > 90) {
      return "#F57C00";
    } else if (temperature > 85) {
      return "#FFA000";
    } else if (temperature > 80) {
      return "#FFC107";
    } else if (temperature > 75) {
      return "#FFD54F";
    } else if (temperature > 70) {
      return "#FFEE58";
    } else if (temperature > 65) {
      return "#FFF176";
    } else if (temperature > 60) {
      return "#E0F7FA";
    } else if (temperature > 55) {
      return "#B2EBF2";
    } else if (temperature > 50) {
      return "#80DEEA";
    } else if (temperature > 45) {
      return "#4DD0E1";
    } else if (temperature > 40) {
      return "#26C6DA";
    } else if (temperature > 35) {
      return "#00BCD4";
    } else if (temperature > 30) {
      return "#00ACC1";
    } else if (temperature > 25) {
      return "#29B6F6";
    } else if (temperature > 20) {
      return "#03A9F4";
    } else if (temperature > 15) {
      return "#039BE5";
    } else if (temperature > 10) {
      return "#0288D1";
    } else if (temperature > 5) {
      return "#0277BD";
    } else {
      return "#01579B";
    }
  }
}
