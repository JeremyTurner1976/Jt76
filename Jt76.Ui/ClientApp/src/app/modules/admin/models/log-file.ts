export interface ILogFile {
  fileName: string;
  fileLocation: string;
}


export class LogFile implements ILogFile {
  fileName = "";
  fileLocation = "";
}
