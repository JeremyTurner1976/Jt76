import { Injectable } from "@angular/core";

import {
  MatSnackBar,
  MatSnackBarConfig,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
  MatSnackBarRef
} from "@angular/material";

import { AlertComponent }
  from "../components/alert/alert.component";
import { environment }
  from "../../../environments/environment";

@Injectable()
export class AlertService {

  verticalPosition: MatSnackBarVerticalPosition = "bottom";
  horizontalPosition: MatSnackBarHorizontalPosition = "right";
  duration = 1200;

  matSnackBarRef: MatSnackBarRef<AlertComponent>;
  matSnackBarConfig: MatSnackBarConfig;

  constructor(public snackBar: MatSnackBar) {
  }

  debug(
    message: string,
    additionalInformation: string = "") {

    if (environment.logLevel === "verbose") {
      this.matSnackBarRef =
        this.openAlert(
          message,
          additionalInformation,
          "alert-debug");
    }
  }

  info(
    message: string,
    additionalInformation: string = "") {

    this.matSnackBarRef =
      this.openAlert(
        message,
        additionalInformation,
        "alert-info");
  }

  warning(
    message: string,
    additionalInformation: string = "") {

    this.matSnackBarRef =
      this.openAlert(
        message,
        additionalInformation,
        "alert-warning");
  }

  error(
    message: string,
    additionalInformation: string = "") {

    this.matSnackBarRef =
      this.openAlert(
        message,
        additionalInformation,
        "alert-error");
  }

  private openAlert(
    message: string,
    additionalInformation: string,
    alertClass: string) {

    return this.snackBar.openFromComponent(
      AlertComponent,
      {
        data: {
          message: message,
          additionalInformation: additionalInformation,
          alertClass: alertClass
        },
        verticalPosition: this.verticalPosition,
        horizontalPosition: this.horizontalPosition,
        duration: this.duration
      });
  }
}
