import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sky-con',
  templateUrl: './sky-con.component.html',
  styleUrls: ['./sky-con.component.scss']
})
export class SkyConComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    var skycons = new Skycons({ "color": "blue" });
    skycons.add("sky-con-canvas", "rain");
    skycons.play();
  }

}
