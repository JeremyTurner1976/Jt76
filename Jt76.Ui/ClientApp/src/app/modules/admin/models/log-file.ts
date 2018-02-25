export interface ILogFile {
  fileName: string;
  fileLocation: string;
  applicationName: string;
  recentFileLines: Array<String>;
  fullFile: Array<String>;
}


export class LogFile implements ILogFile {
  fileName = "";
  fileLocation = "";
  applicationName = "";
  recentFileLines = new Array<String>();
  fullFile = new Array<String>();
}
