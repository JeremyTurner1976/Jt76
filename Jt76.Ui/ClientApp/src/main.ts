import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";

import './polyfills.ts';
import "hammerjs";

if (environment.production) {
  enableProdMode();
}

console.log(environment.versions);

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.log(err));
