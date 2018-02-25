import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AlertService }
  from "../../../shared/services/alert.service";
import { IAppError }
  from "../models/app-error";
import { AppLocalStorageService }
  from "../../../shared/services/app-local-storage.service";
import { BaseService }
  from "../../../shared/abstract/base.service";

@Injectable()
export class ErrorService extends BaseService<IAppError> {

  constructor(
    public http: HttpClient,
    public alertService: AlertService,
    public appLocalStorageService: AppLocalStorageService) {

    super(http, alertService, appLocalStorageService);
    this.singularName = "Error";
    this.dataSetKey = "Errors";
    this.dataUrl = "v1/error";
    this.dataCacheDuration = 15;
  }

  deleteAll(items: IAppError[]) {
    this.clearAllStorage();
    return this.http.delete(
      (this.dataUrl + "/deleteAll")).subscribe(
      () => {
        this.alertService.info("All items deleted");
      });
  }
}
