export interface ILogFile {
  fileName: string;
  fileLocation: string;
  applicationName: string;
  recentFileLines: Array<String>;
}

export class LogFile implements ILogFile {
  fileName = "";
  fileLocation = "";
  applicationName = "";
  recentFileLines = new Array<String>();
}
