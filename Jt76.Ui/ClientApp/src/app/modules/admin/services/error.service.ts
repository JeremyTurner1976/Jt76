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
import { KeyValue }
  from "../../../shared/models/key-value";
import * as moment from "moment";

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
    return this.http.delete(
      (this.dataUrl + "/deleteAll")).subscribe(
      () => {
        this.clearAllStorage();
        this.alertService.info("All items deleted");
      });
  }

  getDetailModel(item: IAppError): Array<KeyValue> {
    const keyValues = new Array<KeyValue>();

    keyValues.push(this.getKeyValue(
      "Message",
      item.message
    ));
    keyValues.push(this.getKeyValue(
      "Error Level",
      item.source
    ));

    keyValues.push(this.getKeyValue(
      "Source",
      item.message
    ));
    keyValues.push(this.getKeyValue(
      "Additional Information",
      item.additionalInformation
    ));

    keyValues.push(this.getKeyValue(
      "Created By",
      item.createdBy
    ));
    keyValues.push(this.getKeyValue(
      "Created Date",
      moment(item.createdDate)
      .format("dddd, MMMM Do, h:mm a")
    ));

    return keyValues;
  }
}
