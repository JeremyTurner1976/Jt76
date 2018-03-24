import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ErrorService } from "../../services/error.service";
import { IAppError, AppError } from "../../models/app-error";
import { BaseDataComponent }
  from "../../../../shared/abstract/base-data-component";

@Component({
  selector: "app-error",
  templateUrl: "./error.component.html",
  styleUrls: ["./error.component.scss"]
})

export class ErrorComponent
  extends BaseDataComponent
  implements OnInit {

  id: number;  
  error: AppError = new AppError();

  constructor(
    private readonly errorService: ErrorService,
    private readonly route: ActivatedRoute) {

    super();
    this.route.params.subscribe(
      params => {
        // ReSharper disable once TsResolvedFromInaccessibleModule
        this.id = params.id;
      }
    );
  }

  getData() {
    this.errorService.getItem(this.id).subscribe(
      (data: IAppError) => {
        this.mapData(data);
      });
  }

  refreshData() {
    this.errorService.refreshItem(this.id).subscribe(
      (data: IAppError) => {
        this.mapData(data);
      });
  }

  mapData(data: IAppError) {
    this.error = data;
  }

  clearData() {
    this.error = new AppError();
  }
}
