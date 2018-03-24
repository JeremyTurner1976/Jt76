import { Component, OnInit } from "@angular/core";
import { IAppError, AppError } from "../models/app-error";
import { ErrorService } from "../services/error.service";
import { BaseDataComponent }
  from "../../../shared/abstract/base-data-component";

@Component({
  selector: "app-errors",
  templateUrl: "./errors.component.html",
  styleUrls: ["./errors.component.scss"]
})

export class ErrorsComponent
  extends BaseDataComponent
  implements OnInit {

  errors = new Array<AppError>();

  constructor(
    private readonly errorService: ErrorService
  ) {
    super();
  }

  getData() {
    this.errorService.getAll().subscribe(
      (data: IAppError[]) => {
        this.mapData(data);
      });
  }

  refreshData() {
    this.errorService.refreshAll().subscribe(
      (data: IAppError[]) => {
        this.mapData(data);
      });
  }

  mapData(data: IAppError[]) {
    this.errors = data;
  }

  clearData() {
    this.errors = new Array<AppError>();
  }

  //todo paged

  clearAll() {
    this.errorService.deleteAll(this.errors);
    this.errors = new Array<AppError>();
  }
}


