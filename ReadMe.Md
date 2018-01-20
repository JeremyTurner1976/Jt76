https://docs.microsoft.com/en-us/aspnet/core/spa/
dotnet new --install Microsoft.DotNet.Web.Spa.ProjectTemplates::2.0.0-rc1-final

https://github.com/aspnet/JavaScriptServices/issues/1288#issuecomment-346003334
Make sure you have .NET Core 2.0.0+ SDK installed. Check this by running dotnet --version in a command prompt. Also you need Node.js installed (check by running node -v).

In your command prompt, run:

dotnet new --install Microsoft.DotNet.Web.Spa.ProjectTemplates::2.0.0-preview1-final
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

--Once this is all set up
You can delete ClientApp Folder, goto Jt76.Ui and ng new ClientApp -> THEN Delete NodeModules as this is kept at the base level
