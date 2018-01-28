import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }
  from '@angular/forms';

//Shared Helpers
import { AppCommonModule } from "./app-common.module";
import { AppExceptionsModule } from "./app-exceptions.module";

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
  declarations: []
})
export class SharedModule {
}
