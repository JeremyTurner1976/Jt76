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
  countIsZero: boolean = false;

  constructor(
    private errorService: ErrorService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.errorService.getAll().subscribe(
        (data: IAppError[]) => {
          this.errors = data;
          this.countIsZero = data.length === 0;
        });
    });
  }

  //todo paged

  refresh() {
    this.errors = new Array<AppError>();
    this.errorService.refreshAll().subscribe(
      (data: IAppError[]) => {
        this.errors = data;
        this.countIsZero = data.length === 0;
    });
  }

  clearAll() {
    this.errorService.deleteAll(this.errors);
    this.errors = new Array<AppError>();
    this.countIsZero = true;
  }
}


