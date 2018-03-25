import {
  Component,
  ViewChild,
  AfterViewInit
} from "@angular/core";
import {
  MatPaginator,
  MatSort,
  MatTableDataSource,
  PageEvent
  } from "@angular/material";
import { IAppError } from "../models/app-error";
import { ErrorService } from "../services/error.service";
import { BaseDataComponent } 
  from "../../../shared/abstract/base-data-component";

@Component({
  selector: "app-errors",
  templateUrl: "./errors.component.html",
  styleUrls: ["./errors.component.scss"]
})
export class ErrorsComponent
extends BaseDataComponent
implements AfterViewInit {

  // Mat table
  errors = new Array<IAppError>();
  dataSource = new MatTableDataSource<IAppError>(this.errors);
  displayedColumns = [
    "createdDate",
    "createdBy",
    "message",
    "info"
  ];

  // Mat paginator
  pageEvent: PageEvent;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  length = 100;
  pageSize = 10;
  pageSizeOptions = [
    5,
    10,
    25,
    100
  ];

  // Mat sort
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private readonly errorService: ErrorService
  ) {
    super();
    this.getData();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
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
    this.setupPaging();
  }

  clearData() {
    this.errors = new Array<IAppError>();
    this.setupPaging();
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.dataSource.filter = filterValue;
  }

  clearAll() {
    this.errorService.deleteAll(this.errors);
    this.clearData();
  }

  setupPaging() {
    this.dataSource = new MatTableDataSource<IAppError>(this.errors);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
}
