import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";
import "rxjs/add/operator/map";
import { AlertService }
  from "../services/alert.service";
import { AppLocalStorageService }
  from "../services/app-local-storage.service";


@Injectable()
export class BaseService<T> {
  singularName: string;
  dataSetKey: string;
  dataUrl: string;
  dataCacheDuration: number;

  constructor(
    public http: HttpClient,
    public alertService: AlertService,
    public appLocalStorageService: AppLocalStorageService) {
  }

  clearAllStorage() {
    this.appLocalStorageService.deleteLocalValue(this.dataSetKey);
  }

  get(): Observable<T> {
    const lastCacheAge =
      this.appLocalStorageService.getLocalCacheAge(this.dataSetKey);
    if (lastCacheAge == null
      || isNaN(lastCacheAge)
      || lastCacheAge >= this.dataCacheDuration) {
      return this.refresh();
    } else {
      const data =
        ((this.appLocalStorageService
          .getLocalValue(this.dataSetKey)) as T);

      this.alertService.debug(
        `${this.dataSetKey}` + " loaded from Local Storage");

      return Observable.of(data);
    }
  }

  refresh(): Observable<T> {
    this.clearAllStorage();
    return this.http.get(this.dataUrl)
      .map(
        (data) => {
          const response = ((data) as T);

          this.appLocalStorageService.saveLocalValue(
            this.dataSetKey,
            response);

          this.alertService.debug(
            `${this.dataSetKey}` + " loaded");

          return response;
        });
  }

  getAll(): Observable<T[]> {
    const lastCacheAge =
      this.appLocalStorageService.getLocalCacheAge(this.dataSetKey);
    if (lastCacheAge == null
      || isNaN(lastCacheAge)
      || lastCacheAge >= this.dataCacheDuration) {
      return this.refreshAll();
    } else {
      const data =
        ((this.appLocalStorageService
          .getLocalValue(this.dataSetKey)) as T[]);

      this.alertService.debug(
        `${data.length} ${this.dataSetKey}`
          + " loaded from Local Storage");

      return Observable.of(data);
    }
  }

  refreshAll(): Observable<T[]> {
    this.clearAllStorage();
    return this.http.get(this.dataUrl)
      .map(
        (data) => {
          const response = ((data) as T[]);

          this.appLocalStorageService.saveLocalValue(
            this.dataSetKey,
            response);

          this.alertService.debug(
            `${response.length} ${this.dataSetKey}` + " loaded");

          return response;
      });
  }

  getItem(
    id: number,
    comparator: any = ((item) => item.id == id)
  ): Observable<T> {
    const lastCacheAge =
      this.appLocalStorageService.getLocalCacheAge(this.dataSetKey);
    if (lastCacheAge == null
      || isNaN(lastCacheAge)
      || lastCacheAge >= this.dataCacheDuration) {

      return this.refreshItem(id);
    } else {
      const data =
        ((this.appLocalStorageService.getLocalValue(this.dataSetKey)) as T[]);

      const matchedItems = data.filter(comparator);
      if (matchedItems.length) {
        this.alertService.debug(
          `${this.singularName} loaded from Local Storage`);

        return Observable.of(matchedItems[0]);
      }

      return this.refreshItem(id);
    }
  }

  refreshItem(id: number): Observable<T> {
    return this.http.get(this.dataUrl + `/${id}`)
      .map(
        (data) => {
          const response = ((data) as T);

          this.alertService.debug(
            `${this.singularName} loaded`);

          return response;
      });
  }

}
