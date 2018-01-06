namespace Jt76.WebApi
{
	using System;
	using Data;
	using Identity;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;

	public class Program
	{
		public static void Main(string[] args)
		{
			IWebHost host = BuildWebHost(args);

			using (IServiceScope scope = host.Services.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;

				DatabaseInitializer databaseInitializer = 
					services.GetRequiredService<DatabaseInitializer>();
				databaseInitializer.SeedAsync().Wait();

				IdentityDatabaseInitializer identityDatabaseInitializer = 
					services.GetRequiredService<IdentityDatabaseInitializer>();
				identityDatabaseInitializer.SeedAsync().Wait();
			}

			host.Run();
		}


		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}





