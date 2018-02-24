import {
  ErrorHandler,
  Inject,
  Injector,
  Injectable
} from "@angular/core";
import {
  HttpClient,
  HttpErrorResponse
} from "@angular/common/http";
import { AppError }
  from "./modules/admin/models/app-error";
import { AlertService }
  from "./shared/services/alert.service";

@Injectable()
export class AppExceptionsHandler implements ErrorHandler {

  constructor(
    private http: HttpClient,
    @Inject(Injector) private injector: Injector) {
  }

  //Get alert service to avoid cyclic dependency
  private get alertService(): AlertService {
    return this.injector.get(AlertService);
  }

  handleError(error: Error) {
    const appError = new AppError();

    if (error instanceof HttpErrorResponse) {
      console.error("Http Error: ", error);
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
      console.error("Application Error: ", error);
      appError.message = error.message;
      appError.source = error.name;
      appError.errorLevel = "Error";
      appError.additionalInformation = error.toString();
      appError.stackTrace = error.stack;
      appError.createdBy = "Client";
      appError.createdDate = new Date();   
    }

    this.alertService.error(
      appError.source,
      appError.message);

    this.http.post("v1/error", appError)
      .subscribe(
        () => {},
        () => {
          this.alertService.error(
            appError.source,
            "Error did not save");
        });
  }
}
