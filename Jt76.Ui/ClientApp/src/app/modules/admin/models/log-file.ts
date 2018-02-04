export interface ILogFile {
  fileName: string;
  fileLocation: string;
  applicationName: string;
}


export class LogFile implements ILogFile {
  fileName = "";
  fileLocation = "";
  applicationName = "";
}
