import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AppError } from "./models/app-error";
import { ErrorHandler, Injectable } from '@angular/core';

@Injectable()
export class AppExceptionsHandler implements ErrorHandler {

  constructor(private http: HttpClient) {  }

  handleError(error: Error) {
    const appError = new AppError();

    if (error instanceof HttpErrorResponse) {
      console.error('Http Error: ', error);
      appError.message = error.message;
      appError.source = error.statusText;
      appError.errorLevel = "Error";
      appError.additionalInformation = error.name;
      appError.stackTrace = error.statusText + " " + error.url;
      appError.createdBy = "Client";
      appError.createdDate = new Date();

      if (!navigator.onLine) {
        // Handle offline error
      } else {
        // Handle Http Error (error.status === 403, 404...)
      }

    } else {
      console.error('Application Error: ', error);
      appError.message = error.message;
      appError.source = error.name;
      appError.errorLevel = "Error";
      appError.additionalInformation = error.toString();
      appError.stackTrace = error.stack;
      appError.createdBy = "Client";
      appError.createdDate = new Date();   
    }

    this.http.post('v1/error', appError)
      .subscribe(
        () => {
          console.log("Error Saved");
        },
        () => {
          console.log("Error failed to save.");
        });
  }
}
