import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";
import "rxjs/add/operator/map";
import { AlertService }
  from "../../../shared/services/alert.service";
import { ILogFile }
  from "../models/log-file";
import { AppLocalStorageService }
  from "../../../shared/services/app-local-storage.service";
import { BaseService }
  from "../../../shared/abstract/base.service";


@Injectable()
export class LogFileService extends BaseService<ILogFile> {

  constructor(
    public http: HttpClient,
    public alertService: AlertService,
    public appLocalStorageService: AppLocalStorageService) {

    super(http, alertService, appLocalStorageService);
    this.singularName = "LogFile";
    this.dataSetKey = "LogFiles";
    this.dataUrl = "v1/logFiles";
    this.dataCacheDuration = 60 * 3;
  }

  getEntireFile(logFile: ILogFile): Observable<string[]> {
    return this.http.get(
      "v1/logFiles/GetFileLines?"
        + `fileLocation=${logFile.fileLocation}`
        + `&fileName=${logFile.fileName}`)
      .map(
        (data) => {
          const response = ((data) as string[]);
          this.alertService.debug(
            `${logFile.fileName}`,
            `${response.length}` + " file lines loaded");
          return response;
       });
  }
}
