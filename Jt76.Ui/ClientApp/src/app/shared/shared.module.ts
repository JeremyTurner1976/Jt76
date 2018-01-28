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
