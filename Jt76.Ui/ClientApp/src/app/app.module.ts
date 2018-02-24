import { NgModule, ErrorHandler } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpModule } from "@angular/http";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";

//npm additions
import { Ng2Webstorage } from "ngx-webstorage";

//shared
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

//Injectables
import { AppExceptionsHandler } from
  "./app-exceptions-handler";
import { AppApiInterceptor } from
  "./app-api-interceptor";

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    HttpClientModule,
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
    Ng2Webstorage.forRoot(
      {
        prefix: "jt76Storage",
        separator: "|",
        caseSensitive: false
      }),
    SharedModule,
    AdminModule,
    WeatherModule
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: AppExceptionsHandler
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AppApiInterceptor,
      multi: true
    }
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
}
