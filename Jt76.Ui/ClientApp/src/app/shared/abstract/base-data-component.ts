export abstract class BaseDataComponent {
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
