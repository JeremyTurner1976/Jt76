import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ErrorService } from "../../services/error.service";
import { IAppError, AppError } from "../../models/app-error";

@Component({
  selector: "app-error",
  templateUrl: "./error.component.html",
  styleUrls: ["./error.component.scss"]
})

export class ErrorComponent implements OnInit {
  id: number;  
  error: AppError = new AppError();

  constructor(
    private errorService: ErrorService,
    private route: ActivatedRoute) {

    this.route.params.subscribe(
      params => {
        this.id = params.id;
      }
    );
  }

  ngOnInit() {
    setTimeout(() => {
      this.errorService.getItem(this.id).subscribe(
        (data: AppError) => {
          this.error = data;
        });
    });
  }
}
