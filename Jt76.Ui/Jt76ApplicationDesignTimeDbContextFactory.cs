namespace Jt76.Ui
{
	using System.IO;
	using AutoMapper;
	using Data.DbContexts;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;
	using Microsoft.Extensions.Configuration;

	public class Jt76ApplicationDesignTimeDbContextFactory : IDesignTimeDbContextFactory<Jt76ApplicationDbContext>
	{
		public Jt76ApplicationDbContext CreateDbContext(string[] args)
		{
			Mapper.Reset();

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.Development.json", optional: true)
				.Build();

			var builder = new DbContextOptionsBuilder<Jt76ApplicationDbContext>();

			builder.UseSqlServer(configuration["ConnectionStrings:ApplicationConnection"], b => b.MigrationsAssembly("Jt76.Ui"));

			return new Jt76ApplicationDbContext(builder.Options);
		}
	}
}
