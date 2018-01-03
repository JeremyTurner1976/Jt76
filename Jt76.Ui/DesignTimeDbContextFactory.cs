// ======================================
// Author: Ebenezer Monney
// Email:  info@ebenmonney.com
// Copyright (c) 2017 www.ebenmonney.com
// 
// ==> Gun4Hire: contact@ebenmonney.com
// ======================================

namespace Jt76.Ui
{
	using System.IO;
	using AutoMapper;
	using Data.DbContexts;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class Jt76IdentityDesignTimeDbContextFactory : IDesignTimeDbContextFactory<Jt76IdentityDbContext>
	{
		public Jt76IdentityDbContext CreateDbContext(string[] args)
		{
			Mapper.Reset();

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.Development.json", optional: true)
				.Build();

			var builder = new DbContextOptionsBuilder<Jt76IdentityDbContext>();

			builder.UseSqlServer(configuration["ConnectionStrings:IdentityConnection"], b => b.MigrationsAssembly("Jt76.Ui"));
			builder.UseOpenIddict();

			return new Jt76IdentityDbContext(builder.Options);
		}
	}

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
