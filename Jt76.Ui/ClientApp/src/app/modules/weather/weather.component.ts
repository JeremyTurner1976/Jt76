import { Component, OnInit } from "@angular/core";
import Weatherservice = require("./services/weather.service");
import WeatherService = Weatherservice.WeatherService;
import { WeatherData } from "./models/weather-data";
import { DailyForecast } from "./models/daily-forecast";
import { WeatherForecast } from "./models/weather-forecast";
import * as moment from "moment";

@Component({
  selector: "app-weather",
  templateUrl: "./weather.component.html",
  styleUrls: ["./weather.component.scss"]
})

export class WeatherComponent implements OnInit {
  isLoaded: boolean = false;
  weatherData = new WeatherData();
  currentWeather = new WeatherForecast();
  weatherForecasts = new Array<WeatherForecast>();
  dailyForecasts = new Array<DailyForecast>();

  constructor(
    private weatherService: WeatherService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.weatherService.get().subscribe(
        (data: WeatherData) => {
          this.mapData(data);
        });
    });
  }

  mapData(data: WeatherData) {
    const dayFormat = "dddd";
    const forecastCount = 8;
    this.weatherData = data;
    const forecasts = data.weatherForecasts;
    this.weatherForecasts = forecasts;

    const now = moment();
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
        var matchingForecasts =
          forecasts.filter(
            (item) => {
              return moment(item.startDateTime).format(dayFormat) === day;
            });

        var dailyForecastCount = matchingForecasts.length;
        if (dailyForecastCount) {
          var dailyForecast = new DailyForecast();

          matchingForecasts.forEach(
            (forecast) => {

              var startDateTime = moment(forecast.startDateTime);
              if ((now.format(dayFormat) === startDateTime.format(dayFormat) &&
                  now.date() === moment(forecast.startDateTime).date()) ||
                now.format(dayFormat) !== startDateTime.format(dayFormat)) {

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
                dailyForecast.totalPrecipitationVolume += forecast.precipitationVolume;
                dailyForecast.avgTemperature += forecast.temperature;
                dailyForecast.avgWindDirection += forecast.windDirection;
                dailyForecast.maxWindspeed =
                  dailyForecast.maxWindspeed < forecast.windspeed
                  ? forecast.windspeed
                  : dailyForecast.maxWindspeed;
                dailyForecast.minWindspeed =
                  forecast.windspeed < dailyForecast.minWindspeed
                  ? forecast.windspeed
                  : dailyForecast.minWindspeed;
                dailyForecast.descriptions.push(forecast.description);
                dailyForecast.skyCons.push(forecast.skyCon);
                dailyForecast.times.push(
                  moment(forecast.startDateTime).format("h:mm a"));
                dailyForecast.temperatures.push(
                  ((forecast.minimumTemperature + forecast.maximumTemperature) / 2).toFixed(0));
              }
            });

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

          if (dailyForecast.descriptions.length !== forecastCount) {
            while (dailyForecast.descriptions.length !== forecastCount) {
              dailyForecast.descriptions.unshift("");
              dailyForecast.skyCons.unshift("");
              dailyForecast.times.unshift("");
              dailyForecast.temperatures.unshift("");
            }
          }

          dailyForecasts.push(dailyForecast);
        }
      });

    this.dailyForecasts = dailyForecasts;
    this.isLoaded = true;
  }

  refresh() {
    this.isLoaded = false;
    this.weatherData = new WeatherData();
    this.currentWeather = new WeatherForecast();
    this.weatherForecasts = new Array<WeatherForecast>();

    this.weatherService.refresh().subscribe(
      (data: WeatherData) => {
        this.mapData(data);
      });
  }
}
