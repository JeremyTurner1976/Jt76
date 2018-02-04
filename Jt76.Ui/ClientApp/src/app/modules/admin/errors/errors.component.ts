import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.scss']
})
export class ErrorsComponent implements OnInit {

  errors: any = [];  

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get('v1/error')
      .subscribe(
        (data) => {
          this.errors = data;
          this.errors.forEach(
            error => {
              error.isVisible = false;
            });
        });
  }

  showInfo(error) {
    //here
    console.log("here");
    console.log(error);
  }

}
