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
import { AppLocalStorageService }
  from "./services/app-local-storage.service";

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
    AlertService,
    AppLocalStorageService
  ],
  entryComponents: [
    AlertComponent
  ]
})
export class SharedModule {
}
