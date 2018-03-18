import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AlertService }
  from "../../../shared/services/alert.service";
import { IWeatherData }
  from "../models/weather-data";
import { AppLocalStorageService }
  from "../../../shared/services/app-local-storage.service";
import { BaseService }
  from "../../../shared/abstract/base.service";

@Injectable()
export class WeatherService extends BaseService<IWeatherData> {

  constructor(
    public http: HttpClient,
    public alertService: AlertService,
    public appLocalStorageService: AppLocalStorageService) {

    super(http, alertService, appLocalStorageService);
    this.singularName = "WeatherForecast";
    this.dataSetKey = "WeatherForecasts";
    this.dataUrl = "v1/weatherData/darkSkyWeatherForecasts";
    //this.dataUrl = "v1/weatherData/openWeatherForecasts";
    this.dataCacheDuration = 30;
  }
}
