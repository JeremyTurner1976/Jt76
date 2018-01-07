namespace Jt76.WebApi
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
				.AddJsonFile("appsettings.Development.json", true)
				.Build();

			DbContextOptionsBuilder<IdentityDbContext> builder = new DbContextOptionsBuilder<IdentityDbContext>();

			builder.UseSqlServer(
				configuration.GetConnectionString("IdentityConnection"),
				b => b.MigrationsAssembly("Jt76.WebApi"));
			builder.UseOpenIddict();

			return new IdentityDbContext(builder.Options);
		}
	}
}