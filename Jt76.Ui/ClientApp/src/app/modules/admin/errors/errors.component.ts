import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AppError } from "../models/app-error";
import { AlertService } from "../../../shared/services/alert.service";

@Component({
  selector: "app-errors",
  templateUrl: "./errors.component.html",
  styleUrls: ["./errors.component.scss"]
})

export class ErrorsComponent implements OnInit {

  errors: AppError[] = new Array<AppError>();  

  constructor(
    private http: HttpClient,
    private alertService: AlertService) { }

  ngOnInit() {
    this.http.get("v1/error")
      .subscribe(
        (data) => {
          this.errors = ((data) as AppError[]);

          this.alertService.debug(
            `${this.errors.length} Errors Loaded`);
        });
  }

}
