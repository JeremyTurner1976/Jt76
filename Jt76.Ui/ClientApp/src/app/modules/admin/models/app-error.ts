export interface IAppError {
  message: string;
  errorLevel: string;
  source: string;
  additionalInformation: string;
  stackTrace: string;
  createdBy: string;
  createdDate: Date;
  updatedBy: string;
  updatedDate: Date;
}


export class AppError implements IAppError {
  message = "";
  errorLevel = "";
  source = "";
  additionalInformation = "";
  stackTrace = "";
  createdBy = "";
  createdDate = new Date();
  updatedBy = "";
  updatedDate = new Date();
}
