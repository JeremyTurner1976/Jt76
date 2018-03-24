import { Component, OnInit } from "@angular/core";
import { WeatherService } from "./services/weather.service";
import { WeatherData } from "./models/weather-data";
import { WeatherForecast } from "./models/weather-forecast";
import { DailyForecast } from "./models/daily-forecast";
import { BaseWeatherComponent } from "./abstract/base-weather-component";

@Component({
  selector: "app-weather",
  templateUrl: "./weather.component.html",
  styleUrls: ["./weather.component.scss"]
})

export class WeatherComponent
  extends BaseWeatherComponent
  implements OnInit {

  weatherData = new WeatherData();
  currentWeather = new WeatherForecast();
  weatherForecasts = new Array<WeatherForecast>();
  dailyForecasts = new Array<DailyForecast>();

  constructor(weatherService: WeatherService) {
    super(weatherService);
  }

  mapData(data: WeatherData) {
    this.weatherData = data;
    this.weatherForecasts = data.weatherForecasts;
    this.dailyForecasts =
      this.weatherService.getDailyForecasts(data.weatherForecasts);
  }

  clearData() {
    this.weatherData = new WeatherData();
    this.currentWeather = new WeatherForecast();
    this.weatherForecasts = new Array<WeatherForecast>();
  }
}
