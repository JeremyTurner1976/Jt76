import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LogFile } from '../models/log-file'

@Component({
  selector: "app-log-files",
  templateUrl: "./log-files.component.html",
  styleUrls: ["./log-files.component.scss"]
})
export class LogFilesComponent implements OnInit {
  step = -1;
  lineCount = 120;
  loadingDetails: Boolean = false;
  logFiles = new Array<LogFile>();
  fileLines = new Array<string>();

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get("v1/logFiles")
      .subscribe(
        (data) => {
          var logFiles = data;
          Object.defineProperty(
            this,
            "logFiles",
            {
              get() {
                return logFiles;
              },
              set(value) {
                logFiles = value;
              }
            });
        });
  }

  logFileClicked(logFile) {
    if (!this.loadingDetails) {
      this.http.get(
          `v1/logFiles/GetLastFileLines?fileLocation=${logFile.fileLocation}&fileName=${logFile.fileName}&count=${this
          .lineCount}`
        )
        .subscribe(
          (data) => {
            var fileLines = data;
            Object.defineProperty(this,
              "fileLines",
              {
                get() {
                  return fileLines;
                },
                set(value) {
                  fileLines = value;
                }
              });
          });
    }
  }

  detailedLogFileClicked(logFile) {
    this.loadingDetails = true;
    this.http.get(
        `v1/logFiles/GetFileLines?fileLocation=${logFile.fileLocation}&fileName=${logFile.fileName}`
      )
      .subscribe(
        (data) => {
          var fileLines = data;
          Object.defineProperty(this,
            "fileLines",
            {
              get() { return fileLines; },
              set(value) { fileLines = value; }
            });
          this.loadingDetails = false;
        },
        (error) => {
          this.loadingDetails = false;
          throw error;
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
    this.step = this.logFiles.length - 1;
  }

}
