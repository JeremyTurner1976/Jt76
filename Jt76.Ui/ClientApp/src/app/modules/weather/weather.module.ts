import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { SharedModule } from
  "../../shared/shared.module";

import { SkyConComponent }
  from "./components/sky-con/sky-con.component";
import { WeatherComponent }
  from "./weather.component";
import { WeatherPanelComponent }
  from "./weather-panel/weather-panel.component";

import { WeatherService } from
  "./services/weather.service";

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {
        path: "weather-panel",
        component: WeatherPanelComponent
      },
      {
        path: "weather",
        component: WeatherComponent
      }
    ])
  ],
  declarations: [
    SkyConComponent,
    WeatherComponent,
    WeatherPanelComponent
  ],
  providers: [
    WeatherService
  ]
})
export class WeatherModule { }
