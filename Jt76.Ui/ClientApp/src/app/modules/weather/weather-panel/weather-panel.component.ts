import {
  Component,
  IterableDiffers,
  DoCheck,
  ElementRef,
  ViewChild
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { WeatherService } from "../services/weather.service";
import { WeatherData } from "../models/weather-data";
import { WeatherForecast } from "../models/weather-forecast";
import { BaseWeatherComponent }
  from "../abstract/base-weather-component";
import * as moment from "moment";
import * as ChartJs from "chart.js";

@Component({
  selector: "app-weather-panel",
  templateUrl: "./weather-panel.component.html",
  styleUrls: ["./weather-panel.component.scss"]
})
export class WeatherPanelComponent
extends BaseWeatherComponent
  implements DoCheck {

  day: string;
  graphData: any;
  chartOptions: any;
  todaysForecasts = new Array<WeatherForecast>();
  differ: any;

  @ViewChild("WeatherGraph") weatherGraph: ElementRef;

  constructor(
    weatherService: WeatherService,
    private readonly route: ActivatedRoute,
    private readonly iterableDifferences: IterableDiffers) {

    super(weatherService);
    this.route.params.subscribe(
      params => {
        // ReSharper disable once TsResolvedFromInaccessibleModule
        this.day = params.day;
      });

    this.getData();
    this.differ = iterableDifferences.find([]).create(null);
  }

  mapData(data: WeatherData) {
    const todaysForecasts =
      this.weatherService
      .getForecastsForDay(data.weatherForecasts, this.day);

    const labels = [];
    const weatherData = [];
    todaysForecasts.forEach(
      (item) => {
        labels.push([
          moment(item.startDateTime).format("M/D h:mm a"),
          item.description]);
        weatherData.push({
          x: moment(item.startDateTime).date(),
          y: +((item.maximumTemperature + item.minimumTemperature) / 2)
            .toFixed(0)
        });
      });

    this.graphData = {
      labels: labels,
      datasets: [
        {
          label: "Temperature",
          data: weatherData,

          backgroundColor: ["rgba(54, 162, 235, 0.2)"],
          borderColor: ["rgba(54, 162, 235, 1)"],
          borderWidth: 1,
          lineTension: 0, // set to 0 means - No bezier

          pointBorderColor: "rgba(75,192,192,1)",
          pointBackgroundColor: "#fff",
          pointBorderWidth: 1,
          pointHoverRadius: 5,
          pointHoverBackgroundColor: "rgba(75,192,192,1)",
          pointHoverBorderColor: "rgba(220,220,220,1)",
          pointHoverBorderWidth: 2,
          pointRadius: 4,
          pointHitRadius: 10
        }
      ]
    };

    this.chartOptions = {
      legend: {
        display: true
      },
      tooltips: {
        mode: "label"
      },

      responsive: true,
      maintainAspectRatio: false,
      scales: {
        yAxes:[{ticks: { beginAtZero: true }}]
      }
    };

    this.todaysForecasts = todaysForecasts;
  }

  clearData() {
    this.todaysForecasts = new Array<WeatherForecast>();
  }

  ngDoCheck() {
    const change = this.differ.diff(this.todaysForecasts);
    if (change && change.length) {
      this.drawGraph();
    }
  }

  drawGraph() {
    //AfterViewInit is not fired to redraw on data changes
    //this is fired for data array changes
    if (this.weatherGraph) {
      this.drawGraphNow();
    } else {
      setTimeout(() => {
          this.drawGraphNow();
        },
        150);
    }
  }

  drawGraphNow() {
    const context = this.weatherGraph.nativeElement.getContext("2d");
    const myChart =
      new ChartJs.Chart(
        context,
        {
          type: "line",
          data: this.graphData,
          options: this.chartOptions
        });
  }
}
