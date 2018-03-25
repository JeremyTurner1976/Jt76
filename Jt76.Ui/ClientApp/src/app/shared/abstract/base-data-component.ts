import { ViewChild, ElementRef } from "@angular/core";

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
  @ViewChild("StepList") private myScrollContainer: ElementRef;
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
    this.myScrollContainer.nativeElement.scrollTop = 0;
    //mat-sidenav
    this.myScrollContainer.nativeElement.parentElement
      .parentElement.parentElement.parentElement.scrollTop = 0;
  }

  lastStep(array: Array<any>) {
    this.step =
      array.length - 1;

    this.myScrollContainer.nativeElement.scrollTop
      = this.myScrollContainer.nativeElement.scrollHeight;
    //mat-sidenav
    this.myScrollContainer.nativeElement.parentElement
      .parentElement.parentElement.parentElement.scrollTop
        = this.myScrollContainer.nativeElement.scrollHeight;
  }

  hideInfo() {
    this.step = -1;
  }
}
