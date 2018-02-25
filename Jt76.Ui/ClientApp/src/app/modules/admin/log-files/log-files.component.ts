import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LogFile, ILogFile } from "../models/log-file";
import { LogFileService } from "../services/log-file.service";

@Component({
  selector: "app-log-files",
  templateUrl: "./log-files.component.html",
  styleUrls: ["./log-files.component.scss"]
})
export class LogFilesComponent implements OnInit {
  step = -1;
  lineCount = 120;
  loadingDetails: Boolean = false;
  lastFile = new LogFile();
  logFiles = new Array<LogFile>();
  fileLines = new Array<string>();
  countIsZero: boolean = false;

  constructor(
    private logFileService: LogFileService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.logFileService.getAll().subscribe(
        (data: ILogFile[]) => {
          this.logFiles = data;
          this.countIsZero = data.length === 0;
        });
    });
  }

  refresh() {
    this.logFileService.refreshAll().subscribe(
      (data: ILogFile[]) => {
        this.logFiles = data;
        this.countIsZero = data.length === 0;
      });
  }

  detailedLogFileClicked(logFile) {
    this.logFileService.getEntireFile(logFile).subscribe(
      (data: string[]) => {
        logFile.recentFileLines = data;
    });
  }

  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }

  firstStep() {
    this.step = 0;
  }

  lastStep() {
    this.step =
      this.logFiles.length - 1;
  }

  hideInfo() {
    this.step = -1;
  }
}
