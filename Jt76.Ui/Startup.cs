using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jt76.Ui
{
	using System;
	using System.Linq;
	using Common.ConfigSettings;
	using Common.ConfigSettings.EmailSettings;
	using Common.ConfigSettings.LogSettings;
	using Common.Enums;
	using Common.Extensions;
	using Common.Interfaces;
	using Common.Services.EmailService;
	using Common.Services.FileService;
	using Microsoft.AspNetCore.Diagnostics;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json.Serialization;

	public class Startup
    {
	    public Startup(IHostingEnvironment env)
	    {
		    Environment = env;
		    IConfigurationBuilder builder = new ConfigurationBuilder()
			    .SetBasePath(env.ContentRootPath)
			    .AddJsonFile("appsettings.json", true, true)
			    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
			    .AddEnvironmentVariables();
		    Configuration = builder.Build();
	    }

	    public IConfiguration Configuration { get; }
	    private IHostingEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {
			// Add framework services.
	        IMvcBuilder mvcBuilder = services.AddMvc();
	        services.AddCors();
	        mvcBuilder.AddJsonOptions
	        (opts => opts.SerializerSettings.ContractResolver =
		        new CamelCasePropertyNamesContractResolver());

	        services.AddOptions();

	        //Configuration Pocos
	        services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
	        services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
	        services.Configure<LogSettings>(Configuration.GetSection("LogSettings"));

	        //Add custom services
	        services.AddTransient<IEmailService, EmailService>();
	        services.AddSingleton<IFileService, FileService>();
	        services.AddLogging();

			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
	        IApplicationBuilder app,
	        IHostingEnvironment env,
	        ILoggerFactory loggerFactory,
	        IEmailService mailService,
	        IOptions<EmailSettings> emailSettings,
	        IFileService fileService,
	        IOptions<LogSettings> logSettings)
		{           
			/*
			public enum LogLevel
			{
				Debug = 1,
				Verbose = 2,
				Information = 3,
				Warning = 4,
				Error = 5,
				Critical = 6,
				None = int.MaxValue
			}
			*/

			//File Logs
			LogSetting fileLogSetting =
				logSettings.Value.Settings.FirstOrDefault(x => x.Type == LogType.File);
			if (fileLogSetting != null && fileLogSetting.On)
				loggerFactory.AddFileLogger(fileService, fileLogSetting.Level);

			//Email Logs
			LogSetting emailLogSetting =
				logSettings.Value.Settings.FirstOrDefault(x => x.Type == LogType.Email);
			if (emailLogSetting != null && emailLogSetting.On)
				loggerFactory.AddEmailLogger(mailService, emailSettings, emailLogSetting.Level);

			//Development settings
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();

				loggerFactory.AddDebug(LogLevel.Information);
				loggerFactory.AddConsole(Configuration.GetSection("Logging"));

				app.UseExceptionHandler(errorApp =>
				{
					errorApp.Run(async context =>
					{
						context.Response.StatusCode = 500; // or another Status according to Exception Type
						context.Response.ContentType = "text/html";

						IExceptionHandlerFeature error =
							context.Features.Get<IExceptionHandlerFeature>();
						if (error != null)
							await context.Response.WriteAsync(error.Error.ToHtml());
					});
				});
			}
			else //production
			{
				app.UseExceptionHandler("/Home/Error");
			}

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
