
/*
		 * TODO

     --build set of moment directives - app-date, accepts utc true/false - defaults to whatever the error is saved as

			--Implement GeoLocation

			//Implement sass variables and traditional theming
			//Implement global event bus (For theme) - can use observe on local storage, themed alert. Should look into traditional angular methods

      //have this one use data source paging/sorting/etc
			//Implement Authentication - https://social.technet.microsoft.com/wiki/contents/articles/37169.secure-your-netcore-web-applications-using-identityserver-4.aspx
			//implement user admin
			//Implement User Secrets Manager
			//storage cleanup service (Use injectors to get all services, check cache time and delete over on app start)

			//Implement dashboards
			//Clean up references

			//Implement EntityAttributeFramework
			//Implement Pdf and excel outputs

			//Implement tests
			//refactor and add comments

			//Implement prod deploy settings and test
		 */















# Jt76.Ui

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.6.5.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).




https://docs.microsoft.com/en-us/aspnet/core/spa/
dotnet new --install Microsoft.DotNet.Web.Spa.ProjectTemplates::2.0.0-rc1-final

https://github.com/aspnet/JavaScriptServices/issues/1288#issuecomment-346003334
Make sure you have .NET Core 2.0.0+ SDK installed. Check this by running dotnet --version in a command prompt. Also you need Node.js installed (check by running node -v).

In your command prompt, run:

dotnet new --install Microsoft.DotNet.Web.Spa.ProjectTemplates::2.0.0-rc1-final
Now you can create projects with it:

Run mkdir my-new-app
Run cd my-new-app
Run dotnet new angular or dotnet new react or dotnet new reactredux
Verify you really have got the updated template output. If the generated project does not have a Views directory, and its Startup.cs contains a call to app.UseSpa, then you do have the new template.
Run dotnet build (note: on the first run, this will do the NPM restore, which can take a few minutes)
Set the environment variable ASPNETCORE_Environment to Development (e.g., set ASPNETCORE_Environment=Development in Windows (non-Powershell prompt) or export ASPNETCORE_Environment=Development on macOS/Linux)
Run dotnet run
Optionally, if you're on Windows, you can open the generated .csproj in Visual Studio 2017 and run the project from there.

npm install -g @angular-devkit/core
npm install --save @angular-devkit/core

--Once this is all set up
You can delete ClientApp Folder, goto Jt76.Ui and ng new ClientApp -> npm install THEN Delete NodeModules in the UI Project as this is kept at the base level of the solution

## Webstorage services, offers session local and volatile storage types
Json serialized object storage via [Webstorage](https://www.npmjs.com/package/ngx-webstorage).
Note: Array storage may require an overwrite. ie array.push(); array = array;

Great comment for pulling in third party libraries
https://stackoverflow.com/questions/43151704/how-implement-chart-js-in-angular-2

https://material.io/guidelines/style/color.html#color-color-palette
