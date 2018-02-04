import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppError } from '../../../shared/models/app-error';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.scss']
})
export class ErrorsComponent implements OnInit {

  errors: AppError[] = new Array<AppError>();  

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get('v1/error')
      .subscribe(
        (data) => {
          var errors = data;
          Object.defineProperty(
            this,
            "errors",
            {
              get() { return errors; },
              set(value) { errors = value; }
            });
        });
  }

}
