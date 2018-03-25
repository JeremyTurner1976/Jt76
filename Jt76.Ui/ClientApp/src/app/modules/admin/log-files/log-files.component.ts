import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LogFile, ILogFile } from "../models/log-file";
import { LogFileService } from "../services/log-file.service";
import { BaseDataComponent }
  from "../../../shared/abstract/base-data-component";

@Component({
  selector: "app-log-files",
  templateUrl: "./log-files.component.html",
  styleUrls: ["./log-files.component.scss"]
})
export class LogFilesComponent
extends BaseDataComponent {
  lineCount = 120;
  lastFile = new LogFile();
  logFiles = new Array<ILogFile>();
  fileLines = new Array<string>();

  constructor(
    private readonly logFileService: LogFileService
  ) {
    super();
    this.getData();
  }

  getData() {
    this.logFileService.getAll().subscribe(
      (data: ILogFile[]) => {
        this.mapData(data);
      });
  }

  refreshData() {
    this.logFileService.refreshAll().subscribe(
      (data: ILogFile[]) => {
        this.mapData(data);
      });
  }

  mapData(data: ILogFile[]) {
    this.logFiles = data;
  }

  clearData() {
    this.logFiles = new Array<ILogFile>();
    this.fileLines = new Array<string>();
  }

  detailedLogFileClicked(logFile) {
    this.logFileService.getEntireFile(logFile).subscribe(
      (data: string[]) => {
        logFile.recentFileLines = data;
      });
  }
}
