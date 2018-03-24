import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { WeatherService } from "../services/weather.service";
import { WeatherData } from "../models/weather-data";
import { WeatherForecast } from "../models/weather-forecast";
import { BaseWeatherComponent } from "../abstract/base-weather-component";

@Component({
  selector: "app-weather-panel",
  templateUrl: "./weather-panel.component.html",
  styleUrls: ["./weather-panel.component.scss"]
})
export class WeatherPanelComponent
  extends BaseWeatherComponent
  implements OnInit {

  day: string;
  todaysForecasts = new Array<WeatherForecast>();

  constructor(
    weatherService: WeatherService,
    private readonly route: ActivatedRoute) {

    super(weatherService);
    this.route.params.subscribe(
      params => {
        // ReSharper disable once TsResolvedFromInaccessibleModule
        this.day = params.day;
      });
  }

  mapData(data: WeatherData) {
    this.todaysForecasts =
      this.weatherService
        .getForecastsForDay(data.weatherForecasts, this.day);

    //dev
    console.log(this.todaysForecasts);
  }

  clearData() {
    this.todaysForecasts = new Array<WeatherForecast>();
  }
}
