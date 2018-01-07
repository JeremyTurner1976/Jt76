namespace Jt76.WebApi
{
	using System.IO;
	using AutoMapper;
	using Data.DbContexts;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;
	using Microsoft.Extensions.Configuration;

	public class Jt76ApplicationDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			Mapper.Reset();

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.Development.json", true)
				.Build();

			DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();

			builder.UseSqlServer(
				configuration.GetConnectionString("ApplicationConnection"),
				b => b.MigrationsAssembly("Jt76.WebApi"));

			return new ApplicationDbContext(builder.Options);
		}
	}
}