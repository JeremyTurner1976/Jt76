export abstract class BaseDataComponent {
  isLoaded: boolean = true;

  ngOnInit() {
    setTimeout(() => {
      this.getData();
    });
  }

  refresh() {
    this.clearData();
    this.refreshData();
  }

  abstract getData(): void;
  abstract refreshData(): void;
  abstract clearData(): void;
  abstract mapData(data: any): void;
}
