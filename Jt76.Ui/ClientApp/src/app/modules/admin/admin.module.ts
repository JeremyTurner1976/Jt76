import { NgModule } from
  "@angular/core";
import { RouterModule } from
  "@angular/router";

import { SharedModule } from
  "../../shared/shared.module";

import { AdminComponent } from
  "./admin.component";
import { DashboardComponent } from
  "./dashboard/dashboard.component";
import { ErrorsComponent } from
  "./errors/errors.component";
import { ErrorComponent } from
  "./errors/error/error.component";
import { LogFilesComponent } from
  "./log-files/log-files.component";
import { UsersComponent } from
  "./users/users.component";
import { WebApiComponent } from
  "./web-api/web-api.component";

import { ErrorService } from
  "./services/error.service";
import { LogFileService } from
  "./services/log-file.service";

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {
        path: "admin",
        component: AdminComponent,
        children: [
          {
            path: "dashboard",
            component: DashboardComponent
          },
          {
            path: "errors",
            component: ErrorsComponent
          },
          {
            path: "errors/:id",
            component: ErrorComponent
          },
          {
            path: "logFiles",
            component: LogFilesComponent
          },
          {
            path: "users",
            component: UsersComponent
          },
          {
            path: "webApi",
            component: WebApiComponent
          },
          {
            path: "",
            redirectTo: "dashboard",
            pathMatch: "full"
          },
          {
            path: "**",
            redirectTo: "dashboard",
            pathMatch: "full"
          }
        ]
      }
    ])
  ],
  declarations: [
    AdminComponent,
    ErrorsComponent,
    ErrorComponent,
    WebApiComponent,
    LogFilesComponent,
    UsersComponent,
    DashboardComponent
  ],
  providers: [
    ErrorService,
    LogFileService
  ]
})
export class AdminModule { }
