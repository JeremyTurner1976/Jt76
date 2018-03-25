import { Component, OnInit } from "@angular/core";
import { IAppError } from "../models/app-error";
import { ErrorService } from "../services/error.service";
import { MatTableDataSource } from "@angular/material";
import { PageEvent } from '@angular/material';
import { BaseDataComponent }
  from "../../../shared/abstract/base-data-component";

@Component({
  selector: "app-errors",
  templateUrl: "./errors.component.html",
  styleUrls: ["./errors.component.scss"]
})
export class ErrorsComponent
  extends BaseDataComponent {

  //mat table
  errors = new Array<IAppError>();
  dataSource = new MatTableDataSource<IAppError>(this.errors);
  displayedColumns = [
    "createdDate",
    "createdBy",
    "message",
    "info"
  ];

  //mat paginator
  length = 100;
  pageSize = 10;
  pageSizeOptions = [
    5,
    10,
    25,
    100
  ];

  // MatPaginator Output
  pageEvent: PageEvent;

  constructor(
    private readonly errorService: ErrorService
  ) {
    super();
    this.getData();
  }

  getData() {
    this.errorService.getAll().subscribe(
      (data: IAppError[]) => {
        this.mapData(data);
      });
  }

  refreshData() {
    this.errorService.refreshAll().subscribe(
      (data: IAppError[]) => {
        this.mapData(data);
      });
  }

  mapData(data: IAppError[]) {
    this.errors = data;
    this.length = data.length;
    this.dataSource = new MatTableDataSource<IAppError>(data);
  }

  clearData() {
    this.errors = new Array<IAppError>();
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.dataSource.filter = filterValue;
  }

  clearAll() {
    this.errorService.deleteAll(this.errors);
    this.errors = new Array<IAppError>();
  }
}


