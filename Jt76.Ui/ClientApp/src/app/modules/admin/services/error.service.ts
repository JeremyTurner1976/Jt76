import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import { AlertService }
  from "../../../shared/services/alert.service";
import { IAppError }
  from "../models/app-error";
import { AppLocalStorageService }
  from "../../../shared/services/app-local-storage.service";


@Injectable()
export class ErrorService {
  dataKey: string = "Errors";
  dataUrl: string = "v1/error";
  dataCacheDuration: number = 15;

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private appLocalStorageService: AppLocalStorageService) {
  }

  clearAllStorage() {
    this.appLocalStorageService.deleteLocalValue(this.dataKey);
  }

  deleteAll(items: IAppError[]) {
    this.appLocalStorageService.deleteLocalValue(this.dataKey);
    return this.http.delete(
      (this.dataUrl + "/deleteAll")).subscribe(
      () => {
          this.alertService.info("All items deleted");
      });
  }

  getAll(refresh: boolean = false): Observable<IAppError[]> {
    const lastCacheAge = this.appLocalStorageService.getLocalCacheAge(this.dataKey);
    if (refresh || lastCacheAge == null || isNaN(lastCacheAge) || lastCacheAge > this.dataCacheDuration) {
      return this.refreshAll();
    } else {
      const data =
        ((this.appLocalStorageService.getLocalValue(this.dataKey)) as IAppError[]);
      this.alertService.debug(
        `${data.length} ${this.dataKey}` + " loaded from LocalStorage");
      return Observable.of(data);
    }
  }

  refreshAll(): Observable<IAppError[]> {
    this.clearAllStorage();
    return this.http.get(this.dataUrl)
      .map(
      (data) => {
        const response = ((data) as IAppError[]);

          this.appLocalStorageService.saveLocalValue(
            this.dataKey,
            response);

          this.alertService.debug(
            `${response.length} ${this.dataKey}` + " loaded");

          return response;
        });
  }

  getItem(id: number, refresh: boolean = false): Observable<IAppError> {
    const lastCacheAge =
      this.appLocalStorageService.getLocalCacheAge(this.dataKey);
    if (refresh
      || lastCacheAge == null
      || isNaN(lastCacheAge)
      || lastCacheAge > this.dataCacheDuration) {

      return this.refreshItem(id);
    } else {
      const data =
        ((this.appLocalStorageService.getLocalValue(this.dataKey)) as IAppError[]);
      // ReSharper disable once CoercedEqualsUsing
      const matchedItems = data.filter(item => item.id == id);

      if (matchedItems.length) {
        this.alertService.debug("Item loaded from LocalStorage");
        return Observable.of(matchedItems[0]);
      }

      return this.refreshItem(id);
    }
  }

  refreshItem(id: number): Observable<IAppError>  {
    return this.http.get(this.dataUrl + `/${id}`)
      .map(
        (data) => {
          const response = ((data) as IAppError);
          this.alertService.debug("Item loaded");
          return response;
        });
  }
}
