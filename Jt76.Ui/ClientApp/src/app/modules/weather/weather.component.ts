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
  useOpenWeather: boolean = false;
  weatherData = new WeatherData();
  currentWeather = new WeatherForecast();
  weatherForecasts = new Array<WeatherForecast>();
  dailyForecasts = new Array<DailyForecast>();

  constructor(
    private weatherService: WeatherService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.weatherService.setUrl(this.useOpenWeather);
      this.weatherService.get().subscribe(
        (data: WeatherData) => {
          this.mapData(data);
        });
    });
  }

  useOpenWeatherApi(useOpenWeather: boolean) {
    this.useOpenWeather = useOpenWeather;
    this.refresh();
  }

  refresh() {
    this.isLoaded = false;
    this.weatherData = new WeatherData();
    this.currentWeather = new WeatherForecast();
    this.weatherForecasts = new Array<WeatherForecast>();
    this.weatherService.setUrl(this.useOpenWeather);
    this.weatherService.refresh().subscribe(
      (data: WeatherData) => {
        this.mapData(data);
        console.log(data);
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

          dailyForecasts.push(dailyForecast);
        }
      });

    this.dailyForecasts = dailyForecasts;
    this.isLoaded = true;
  }

  getTemperatureColor(temperature: number): string {
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
