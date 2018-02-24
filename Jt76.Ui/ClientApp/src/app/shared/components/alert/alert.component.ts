import { Component, OnInit, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})

export class AlertComponent implements OnInit {

  message: string = "";
  additionalInformation: string = "";
  alertClass: string = "alert-info";

  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) {
    this.message = data.message;
    this.additionalInformation = data.additionalInformation;
    this.alertClass = data.alertClass;
  }

  ngOnInit() {
  }

}
