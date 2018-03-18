import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,

  Input
} from "@angular/core";

@Component({
  selector: "app-sky-con",
  templateUrl: "./sky-con.component.html",
  styleUrls: ["./sky-con.component.scss"]
})
export class SkyConComponent implements OnInit {
  @Input() height = 128;
  @Input() width = 128;
  @Input() icon = "rain";
  @Input() color = "blue";

  @ViewChild("SkyCanvas") skyCanvas: ElementRef;

  ngOnInit() {
    const skycons = new Skycons({
      "color": this.color
    });

    skycons.add(
      this.skyCanvas.nativeElement,
      this.icon);

    skycons.play();
  }

}
