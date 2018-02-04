import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule }
  from '@angular/forms';

//Shared Helpers
import { AppCommonModule } from "./app-common.module";
import { AppExceptionsModule } from "./app-exceptions.module";
import { AppApiInterceptor } from './app-api-interceptor.module';

import { MaterialsModule } from "./materials.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialsModule,
    AppCommonModule,
    AppExceptionsModule
  ],
  providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: AppApiInterceptor,
      multi: true,
    }
  ],
  declarations: []
})
export class SharedModule {
}
