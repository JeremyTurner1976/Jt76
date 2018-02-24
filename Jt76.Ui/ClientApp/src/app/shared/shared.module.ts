import { NgModule }
  from "@angular/core";
import { CommonModule }
  from "@angular/common";
import {
  FormsModule,
  ReactiveFormsModule
} from "@angular/forms";

//shared components
import { AlertComponent } from
  "./components/alert/alert.component";

//shared services
import { AlertService }
  from "./services/alert.service";

//module wrappers
import { MaterialsModule }
  from "./materials.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialsModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialsModule
  ],
  declarations: [
    AlertComponent
  ],
  providers: [
    AlertService
  ],
  entryComponents: [
    AlertComponent
  ]
})
export class SharedModule {
}
