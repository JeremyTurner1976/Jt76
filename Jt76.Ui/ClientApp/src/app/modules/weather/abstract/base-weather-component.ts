import { BaseDataComponent }
  from "../../../shared/abstract/base-data-component";
import { WeatherData } from "../models/weather-data";
import { WeatherService } from "../services/weather.service";

export abstract class BaseWeatherComponent extends BaseDataComponent {
  useOpenWeather: boolean = false;

  constructor(
    public weatherService: WeatherService
  ) {
    super();
    this.useOpenWeather = this.weatherService.isUsingOpenWeather();
  }

  getData() {
    this.isLoaded = false;
    this.weatherService.setUrl(this.useOpenWeather);
    this.weatherService.get().subscribe(
      (data: WeatherData) => {
        this.mapData(data);
        this.isLoaded = true;
      });
  }

  refreshData() {
    this.isLoaded = false;
    this.weatherService.setUrl(this.useOpenWeather);
    this.weatherService.refresh().subscribe(
      (data: WeatherData) => {
        this.mapData(data);
        this.isLoaded = true;
      });
  }

  useOpenWeatherApi(useOpenWeather: boolean) {
    this.useOpenWeather = useOpenWeather;
    this.refresh();
  }
}
