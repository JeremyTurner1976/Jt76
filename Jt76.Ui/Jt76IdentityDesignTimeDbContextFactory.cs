namespace Jt76.Ui
{
	using System.IO;
	using AutoMapper;
	using Identity.DbContexts;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class Jt76IdentityDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
	{
		public IdentityDbContext CreateDbContext(string[] args)
		{
			Mapper.Reset();

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.Development.json", optional: true)
				.Build();

			var builder = new DbContextOptionsBuilder<IdentityDbContext>();

			builder.UseSqlServer(configuration["ConnectionStrings:IdentityConnection"], b => b.MigrationsAssembly("Jt76.Identity"));
			builder.UseOpenIddict();

			return new IdentityDbContext(builder.Options);
		}
	}
}
