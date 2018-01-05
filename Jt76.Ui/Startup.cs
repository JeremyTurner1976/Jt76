namespace Jt76.Ui
{
	using System.Net;
	using AspNet.Security.OAuth.Validation;
	using AspNet.Security.OpenIdConnect.Primitives;
	using AutoMapper;
	using Common.CommonData.Interfaces;
	using Common.Constants;
	using Data;
	using Data.DbContexts;
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
	using Microsoft.AspNetCore.SpaServices.Webpack;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using Swashbuckle.AspNetCore.Swagger;

	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(Configuration["ConnectionStrings:ApplicationConnection"], b => b.MigrationsAssembly("Jt76.Data"));
			});

			services.AddDbContext<IdentityDbContext>(options =>
			{
				options.UseSqlServer(Configuration["ConnectionStrings:IdentityConnection"], b => b.MigrationsAssembly("Jt76.Identity"));
				options.UseOpenIddict();
			});

			// add identity
			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<IdentityDbContext>()
				.AddDefaultTokenProviders();

			// Configure Identity options and password complexity here
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
			}).AddOAuthValidation();


			// Add cors
			services.AddCors();

			// Add framework services.
			services.AddMvc();


			//Todo: ***Using DataAnnotations for validation until Swashbuckle supports FluentValidation***
			//services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());


			//.AddJsonOptions(opts =>
			//{
			//    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			//});



			services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("BearerAuth", new ApiKeyScheme
				{
					Name = "Authorization",
					Description = "Login with your bearer authentication token. e.g. Bearer <auth-token>",
					In = "header",
					Type = "apiKey"
				});

				c.SwaggerDoc("v1", new Info { Title = "Jt76Base.Ui API", Version = "v1" });
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(AuthPolicies.ViewUserByUserIdPolicy, policy => policy.Requirements.Add(new ViewUserByIdRequirement()));

				options.AddPolicy(AuthPolicies.ViewUsersPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers));

				options.AddPolicy(AuthPolicies.ManageUserByUserIdPolicy, policy => policy.Requirements.Add(new ManageUserByIdRequirement()));

				options.AddPolicy(AuthPolicies.ManageUsersPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers));

				options.AddPolicy(AuthPolicies.ViewRoleByRoleNamePolicy, policy => policy.Requirements.Add(new ViewRoleByNameRequirement()));

				options.AddPolicy(AuthPolicies.ViewRolesPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles));

				options.AddPolicy(AuthPolicies.AssignRolesPolicy, policy => policy.Requirements.Add(new AssignRolesRequirement()));

				options.AddPolicy(AuthPolicies.ManageRolesPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageRoles));
			});

			Mapper.Initialize(cfg =>
			{
				cfg.AddProfile<AutoMapperProfile>();
			});


			// Repositories
			services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
			services.AddScoped<IAccountManager, AccountManager>();

			// Auth Policies
			services.AddSingleton<IAuthorizationHandler, ViewUserByIdHandler>();
			services.AddSingleton<IAuthorizationHandler, ManageUserByIdHandler>();
			services.AddSingleton<IAuthorizationHandler, ViewRoleByNameHandler>();
			services.AddSingleton<IAuthorizationHandler, AssignRolesHandler>();

			// DB Creation and Seeding
			services.AddTransient<DatabaseInitializer, DatabaseInitializer>();
			services.AddTransient<IdentityDatabaseInitializer, IdentityDatabaseInitializer>();
		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug(LogLevel.Warning);
			loggerFactory.AddFile(Configuration.GetSection("Logging"));

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true
				});
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}


			//Configure Cors
			//app.UseCors(builder => builder
			//	.AllowAnyOrigin()
			//	.AllowAnyHeader()
			//	.AllowAnyMethod());


			app.UseStaticFiles();
			app.UseAuthentication();


			app.UseExceptionHandler(builder =>
			{
				builder.Run(async context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = MediaTypeNames.ApplicationJson;

					var error = context.Features.Get<IExceptionHandlerFeature>();

					if (error != null)
					{
						string errorMsg = JsonConvert.SerializeObject(new { error = error.Error.Message });
						await context.Response.WriteAsync(errorMsg).ConfigureAwait(false);
					}
				});
			});


			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jt76Base.Ui API V1");
			});


			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapSpaFallbackRoute(
					name: "spa-fallback",
					defaults: new { controller = "Home", action = "Index" });
			});
		}
	}
}
