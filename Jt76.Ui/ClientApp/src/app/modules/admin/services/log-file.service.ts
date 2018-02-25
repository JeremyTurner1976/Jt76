import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import { AlertService }
  from "../../../shared/services/alert.service";
import { ILogFile } from "../models/log-file";
import { AppLocalStorageService }
  from "../../../shared/services/app-local-storage.service";


@Injectable()
export class LogFileService {
  dataKey: string = "LogFiles";
  dataUrl: string = "v1/logFiles";
  lineCount: number = 120;
  dataCacheDuration: number = 60 * 3;

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private appLocalStorageService: AppLocalStorageService) {
  }

  clearAllStorage() {
    this.appLocalStorageService.deleteLocalValue(this.dataKey);
  }

  getAll(refresh: boolean = false): Observable<ILogFile[]> {
    const lastCacheAge = this.appLocalStorageService.getLocalCacheAge(this.dataKey);
    if (refresh || lastCacheAge == null || isNaN(lastCacheAge) || lastCacheAge > this.dataCacheDuration) {
      return this.refreshAll();
    } else {
      const data =
        ((this.appLocalStorageService.getLocalValue(this.dataKey)) as ILogFile[]);
      this.alertService.debug(
        `${data.length} ${this.dataKey}` + " loaded from LocalStorage");
      return Observable.of(data);
    }
  }

  refreshAll(): Observable<ILogFile[]> {
    this.clearAllStorage();
    return this.http.get(this.dataUrl)
      .map(
        (data) => {
          const response = ((data) as ILogFile[]);

          this.appLocalStorageService.saveLocalValue(
            this.dataKey,
            response);

          this.alertService.debug(
            `${response.length} ${this.dataKey}` + " loaded");

          return response;
        });
  }

  getEntireFile(logFile: ILogFile): Observable<string[]> {
    return this.http.get(
      `v1/logFiles/GetFileLines?${`fileLocation=${logFile.fileLocation}`}${`&fileName=${logFile.fileName}`}`
      )
      .map(
        (data) => {
          const response = ((data) as string[]);
          this.alertService.debug(
            `${logFile.fileName},
            ${response.length}` + " file lines loaded");
          return response;
        });
  }
}
