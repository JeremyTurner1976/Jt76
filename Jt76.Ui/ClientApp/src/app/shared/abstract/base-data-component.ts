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

  //step interface
  step = -1;

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

  lastStep(array: Array<any>) {
    this.step =
      array.length - 1;
  }

  hideInfo() {
    this.step = -1;
  }
}
