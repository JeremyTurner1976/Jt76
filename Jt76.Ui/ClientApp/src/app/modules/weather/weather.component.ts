import { Component, OnInit } from "@angular/core";
import Weatherservice = require("./services/weather.service");
import WeatherService = Weatherservice.WeatherService;
import { WeatherData } from "./models/weather-data";
import { WeatherForecast } from "./models/weather-forecast";

@Component({
  selector: "app-weather",
  templateUrl: "./weather.component.html",
  styleUrls: ["./weather.component.scss"]
})

export class WeatherComponent implements OnInit {
  isLoaded: boolean = false;
  weatherData = new WeatherData();
  currentWeather = new WeatherForecast();
  currentTime = new Date();
  weatherForecasts = new Array<WeatherForecast>();

  constructor(
    private weatherService: WeatherService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.weatherService.get().subscribe(
        (data: WeatherData) => {
          this.weatherData = data;
          this.currentWeather = data.weatherForecasts[0];
          this.weatherForecasts = data.weatherForecasts;
          this.isLoaded = true;

          //temp
          console.log(this.weatherData);
          console.log(this.weatherForecasts[0]);
        });
    });
  }

  refresh() {
    this.isLoaded = false;
    this.currentTime = new Date();
    this.weatherData = new WeatherData();
    this.currentWeather = new WeatherForecast();
    this.weatherForecasts = new Array<WeatherForecast>();

    this.weatherService.refresh().subscribe(
      (data: WeatherData) => {
        this.weatherData = data;
        this.currentWeather = data.weatherForecasts[0];
        this.weatherForecasts = data.weatherForecasts;
        this.isLoaded = true;
      });
  }
}
