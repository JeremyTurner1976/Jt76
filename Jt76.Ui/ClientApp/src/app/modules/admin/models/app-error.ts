import { KeyValue }
  from "../../../shared/models/key-value";

export interface IAppError {
  id: number;
  message: string;
  errorLevel: string;
  source: string;
  additionalInformation: string;
  stackTrace: string;
  createdBy: string;
  createdDate: Date;
  updatedBy: string;
  updatedDate: Date;
  detailModel: Array<KeyValue>;
}

export class AppError implements IAppError {
  id = 0;
  message = "";
  errorLevel = "";
  source = "";
  additionalInformation = "";
  stackTrace = "";
  createdBy = "";
  createdDate = new Date();
  updatedBy = "";
  updatedDate = new Date();
  detailModel = new Array<KeyValue>();
}
