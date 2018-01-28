import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";

import "hammerjs";

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.log(err));
