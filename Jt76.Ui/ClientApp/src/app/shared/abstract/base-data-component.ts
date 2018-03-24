export interface IBaseDataComponent {
  refresh(): void;
  getData(): void;
  refreshData(): void;
  clearData(): void;
  mapData(data: any): void;
}

export abstract class BaseDataComponent implements IBaseDataComponent {
  isLoaded: boolean = true;

  refresh() {
    this.clearData();
    this.refreshData();
  }

  abstract getData(): void;
  abstract refreshData(): void;
  abstract clearData(): void;
  abstract mapData(data: any): void;
}
