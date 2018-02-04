namespace Jt76.WebApi
{
	using System;
	using System.Linq;
	using AspNet.Security.OAuth.Validation;
	using AspNet.Security.OpenIdConnect.Primitives;
	using AutoMapper;
	using Common.CommonData.Interfaces;
	using Common.ConfigSettings;
	using Common.ConfigSettings.EmailSettings;
	using Common.ConfigSettings.LogSettings;
	using Common.Constants;
	using Common.Enums;
	using Common.Extensions;
	using Common.Interfaces;
	using Common.Services.EmailService;
	using Common.Services.FileService;
	using Common.Services.LogService.HttpLogger;
	using Data;
	using Data.DbContexts;
	using Data.Services.LoggingServices;
	using ExternalServices.WeatherServices;
	using ExternalServices.WeatherServices.ConfigSettings;
	using Identity;
	using Identity.DbContexts;
	using Identity.Interfaces;
	using Identity.Models;
	using Identity.Models.MappedModels;
	using Identity.Policies;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Diagnostics;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json.Serialization;
	using Swashbuckle.AspNetCore.Swagger;

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

		// This method gets called by the runtime. Use this 
		// method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			IMvcBuilder mvcBuilder = services.AddMvc();
			services.AddCors();
			mvcBuilder.AddJsonOptions
			(opts => opts.SerializerSettings.ContractResolver =
				new CamelCasePropertyNamesContractResolver());

			services.AddMemoryCache(opt => opt.ExpirationScanFrequency =
				TimeSpan.FromMinutes(5));
			services.AddOptions();

			//Add swagger json file gen
			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition(
					"BearerAuth",
					new ApiKeyScheme
					{
						Name = "Authorization",
						Description = "Login with your bearer " +
						              "authentication token. e.g. Bearer <auth-token>",
						In = "header",
						Type = "apiKey"
					});

				options.SwaggerDoc("v1",
					new Info
					{
						Version = "v1",
						Title = "J76 Api",
						Description = "Api and Schema Definitions",
						TermsOfService = "None"
					});
			});

			ConfigureIdentityServices(services);

			services.AddDbContext<ApplicationDbContext>(
				options =>
				{
					options
						.UseSqlServer(
							Configuration.GetConnectionString("ApplicationConnection"),
							builder => builder.MigrationsAssembly("Jt76.WebApi"));
				});

			//Configuration Pocos
			services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
			services.Configure<LogSettings>(Configuration.GetSection("LogSettings"));

			// Application Code

			//http://stackoverflow.com/questions/38138100/what-is-the-difference-between-services-addtransient-service-addscope-and-servi
			//Singleton which creates a single instance throughout the application.
			//It creates the instance for the first time and reuses the same 
			//object in the all calls.

			//Scoped lifetime services are created once per request within the scope.
			//It is equivalent to Singleton 
			//in the current scope.eg. in MVC it creates 1 instance per 
			//each http request but uses the same instance 
			//in the other calls within the same web request.

			//Transient lifetime services are created each time they are requested.
			//This lifetime works best for lightweight, stateless services.

			//Dark Sky Api
			services.AddSingleton<DarkSkyWeatherServiceSettings, DarkSkyWeatherServiceSettings>();
			services.AddScoped<DarkSkyWeatherService, DarkSkyWeatherService>();

			//Open Weather Api
			services.AddSingleton<OpenWeatherServiceSettings, OpenWeatherServiceSettings>();
			services.AddScoped<OpenWeatherService, OpenWeatherService>();

			//Add data classes
			services.AddScoped<UnitOfWork, UnitOfWork>();

			//Add custom services
			services.AddTransient<IEmailService, EmailService>();
			services.AddTransient<IFileService, FileService>();
			services.AddSingleton<IDatabaseLoggingService, DatabaseLoggingService>();

			// DB Creation and Seeding
			services.AddTransient<DatabaseInitializer, DatabaseInitializer>();

			services.AddLogging();
		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			ILoggerFactory loggerFactory,
			IEmailService mailService,
			IOptions<EmailSettings> emailSettings,
			IFileService fileService,
			UnitOfWork unitOfWork,
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

			//database Logs
			LogSetting dataBaseLogSetting =
				logSettings.Value.Settings.FirstOrDefault(x => x.Type == LogType.Database);
			if (dataBaseLogSetting != null && dataBaseLogSetting.On)
				loggerFactory.AddDatabaseLogger(
					app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
						.CreateScope()
						.ServiceProvider,
					dataBaseLogSetting.Level);

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
			app.UseAuthentication();
			app.UseMiddleware<HttpContextLogging>();

			//Bring in Swagger definitions
			app.UseSwagger();

			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint(
					"/swagger/v1/swagger.json",
					"Api and Schema Definitions");
			});

			app.UseCors(corsPolicyBuilder =>
				corsPolicyBuilder
					.WithOrigins("http://localhost:57752")
					.AllowAnyMethod()
					.AllowAnyHeader()
			);

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});
		}

		private void ConfigureIdentityServices(IServiceCollection services)
		{
			// Identity
			services.AddDbContext<IdentityDbContext>(options =>
			{
				options.UseSqlServer(
					Configuration.GetConnectionString("IdentityConnection"),
					b => b.MigrationsAssembly("Jt76.WebApi"));
				options.UseOpenIddict();
			});

			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<IdentityDbContext>()
				.AddDefaultTokenProviders();

			// Configure options and password complexity here
			services.Configure<IdentityOptions>(options =>
			{
				// User settings
				options.User.RequireUniqueEmail = true;

				//    //// Password settings
				//    //options.Password.RequireDigit = true;
				//    //options.Password.RequiredLength = 8;
				//    //options.Password.RequireNonAlphanumeric = false;
				//    //options.Password.RequireUppercase = true;
				//    //options.Password.RequireLowercase = false;

				//    //// Lockout settings
				//    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				//    //options.Lockout.MaxFailedAccessAttempts = 10;

				options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
				options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
				options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
			});

			// Register the OpenIddict services.
			services.AddOpenIddict(options =>
			{
				options.AddEntityFrameworkCoreStores<IdentityDbContext>();
				options.AddMvcBinders();
				options.EnableTokenEndpoint("/connect/token");
				options.AllowPasswordFlow();
				options.AllowRefreshTokenFlow();
				options.DisableHttpsRequirement();
				//options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
				//options.AddSigningKey(new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["STSKey"])));
			});

			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = OAuthValidationDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = OAuthValidationDefaults.AuthenticationScheme;
				})
				.AddOAuthValidation();

			services.AddAuthorization(options =>
			{
				options.AddPolicy(AuthPolicies.ViewUserByUserIdPolicy,
					policy => policy.Requirements.Add(new ViewUserByIdRequirement()));

				options.AddPolicy(AuthPolicies.ViewUsersPolicy,
					policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers));

				options.AddPolicy(AuthPolicies.ManageUserByUserIdPolicy,
					policy => policy.Requirements.Add(new ManageUserByIdRequirement()));

				options.AddPolicy(AuthPolicies.ManageUsersPolicy,
					policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers));

				options.AddPolicy(AuthPolicies.ViewRoleByRoleNamePolicy,
					policy => policy.Requirements.Add(new ViewRoleByNameRequirement()));

				options.AddPolicy(AuthPolicies.ViewRolesPolicy,
					policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles));

				options.AddPolicy(AuthPolicies.AssignRolesPolicy, policy => policy.Requirements.Add(new AssignRolesRequirement()));

				options.AddPolicy(AuthPolicies.ManageRolesPolicy,
					policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageRoles));
			});

			Mapper.Initialize(config => { config.AddProfile<AutoMapperProfile>(); });

			services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
			services.AddScoped<IAccountManager, AccountManager>();

			// Auth Policies
			services.AddSingleton<IAuthorizationHandler, ViewUserByIdHandler>();
			services.AddSingleton<IAuthorizationHandler, ManageUserByIdHandler>();
			services.AddSingleton<IAuthorizationHandler, ViewRoleByNameHandler>();
			services.AddSingleton<IAuthorizationHandler, AssignRolesHandler>();

			services.AddTransient<IdentityDatabaseInitializer, IdentityDatabaseInitializer>();
		}
	}
}