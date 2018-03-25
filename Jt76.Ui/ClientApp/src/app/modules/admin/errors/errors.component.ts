import { Component, OnInit } from "@angular/core";
import { IAppError } from "../models/app-error";
import { ErrorService } from "../services/error.service";
import { BaseDataComponent }
  from "../../../shared/abstract/base-data-component";

@Component({
  selector: "app-errors",
  templateUrl: "./errors.component.html",
  styleUrls: ["./errors.component.scss"]
})

export class ErrorsComponent
  extends BaseDataComponent {

  errors = new Array<IAppError>();

  constructor(
    private readonly errorService: ErrorService
  ) {
    super();
    this.getData();
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
    this.errors = new Array<IAppError>();
  }

  //todo paged

  clearAll() {
    this.errorService.deleteAll(this.errors);
    this.errors = new Array<IAppError>();
  }
}


