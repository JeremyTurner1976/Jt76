namespace Jt76.Ui
{
	using System;
	using Common.Constants;
	using Data;
	using Identity;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var databaseInitializer = services.GetRequiredService<DatabaseInitializer>();
					databaseInitializer.SeedAsync().Wait();

					var identityDatabaseInitializer = services.GetRequiredService<IdentityDatabaseInitializer>();
					identityDatabaseInitializer.SeedAsync().Wait();
				}
				catch (Exception initializationException)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogCritical("Error inititializing databases, during SeedAsync()", initializationException);
					throw;
				}
			}

			host.Run();
		}


		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
