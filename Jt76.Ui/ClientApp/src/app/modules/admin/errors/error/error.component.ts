import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {

  id: number;  
  error: any = {
    html: ""
  };  

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient) {

    this.route.params.subscribe(
      params => {
        this.id = params.id;
      }
    );
  }

  ngOnInit() {
    this.http.get('v1/error/GetAsHtml/' + this.id)
      .subscribe(
        (data) => {
          this.error = data;
        });
  }
}
