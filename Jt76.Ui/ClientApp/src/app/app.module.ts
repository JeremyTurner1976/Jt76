import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';


//Shared components, services, and modules
import { SharedModule } from
  "./shared/shared.module";

//Features
import { AdminModule } from
  "./modules/admin/admin.module";
import { WeatherModule } from
  "./modules/weather/weather.module";
import { DashboardComponent }
  from "./dashboard/dashboard.component";
import { AppComponent } from
  "./app.component";


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    RouterModule.forRoot([
      {
        path: "dashboard",
        component: DashboardComponent
      },
      {
        path: "", redirectTo: "dashboard",
        pathMatch: "full"
      },
      {
        path: "**", redirectTo: "dashboard",
        pathMatch: "full"
      }
    ]),
    SharedModule,
    AdminModule,
    WeatherModule
  ],
  providers: [
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
}
