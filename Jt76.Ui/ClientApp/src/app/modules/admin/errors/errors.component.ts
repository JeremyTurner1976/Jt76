import { Component, OnInit } from "@angular/core";
import { IAppError, AppError } from "../models/app-error";
import { ErrorService } from "../services/error.service";

@Component({
  selector: "app-errors",
  templateUrl: "./errors.component.html",
  styleUrls: ["./errors.component.scss"]
})

export class ErrorsComponent implements OnInit {
  errors: AppError[] = new Array<AppError>();
  loading: boolean = false;

  constructor(
    private errorService: ErrorService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.loading = true;
      this.errorService.getAll().subscribe(
        (data: IAppError[]) => {
          this.errors = data;
          this.loading = false;
        });
    });
  }

  refresh() {
    this.loading = true;
    this.errorService.refreshAll().subscribe(
      (data: IAppError[]) => {
        this.errors = data;
        this.loading = false;
      });
  }

  clearAll() {
    this.errorService.deleteAll(this.errors);
    this.errors = new Array<AppError>();
  }
}


