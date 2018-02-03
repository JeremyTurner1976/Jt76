import { NgModule } from
  '@angular/core';
import { RouterModule } from
  '@angular/router';
import { SharedModule } from
  '../../shared/shared.module';

import { AdminComponent } from
  './admin.component';
import { ErrorsComponent } from
  './errors/errors.component';
import { ErrorComponent } from
  './errors/error/error.component';
import { WebApiComponent } from
  './web-api/web-api.component';

import { ErrorService } from './services/error.service';
import { LogFilesComponent } from './log-files/log-files.component';

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {
        path: 'errors',
        component: ErrorsComponent
      },
      {
        path: 'error/:id',
        component: ErrorComponent
      },
      {
        path: "webApi",
        component: WebApiComponent
      },
      {
        path: "logFiles",
        component: LogFilesComponent
      },
      {
        path: 'admin',
        component: AdminComponent
      }
    ])
  ],
  declarations: [
    AdminComponent,
    ErrorsComponent,
    ErrorComponent,
    WebApiComponent,
    LogFilesComponent
  ],
  providers: [
    ErrorService
  ]
})
export class AdminModule { }
