import { Component, OnInit } from "@angular/core";
import { WeatherService } from "./services/weather.service";
import { WeatherData } from "./models/weather-data";
import { IWeatherForecast } from "./models/weather-forecast";
import { IDailyForecast } from "./models/daily-forecast";
import { BaseWeatherComponent } from "./abstract/base-weather-component";

@Component({
  selector: "app-weather",
  templateUrl: "./weather.component.html",
  styleUrls: ["./weather.component.scss"]
})

export class WeatherComponent
  extends BaseWeatherComponent {

  weatherData = new WeatherData();
  weatherForecasts = new Array<IWeatherForecast>();
  dailyForecasts = new Array<IDailyForecast>();

  constructor(weatherService: WeatherService) {
    super(weatherService);
    this.getData();
  }

  mapData(data: WeatherData) {
    this.weatherData = data;
    this.weatherForecasts = data.weatherForecasts;
    this.dailyForecasts =
      this.weatherService.getDailyForecasts(data.weatherForecasts);
  }

  clearData() {
    this.weatherData = new WeatherData();
    this.weatherForecasts = new Array<IWeatherForecast>();
    this.dailyForecasts = new Array<IDailyForecast>();
  }
}
