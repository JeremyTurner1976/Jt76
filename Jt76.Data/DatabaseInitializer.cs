namespace Jt76.Data
{
	using System;
	using System.Threading.Tasks;
	using Common.CommonData.Interfaces;
	using DbContexts;
	using Factories;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;
	using Models;

	public class DatabaseInitializer : IDatabaseInitializer
	{
		private const int dataPopulationCount = 3;
		private readonly ApplicationDbContext _applicationContext;
		private readonly ILogger _logger;

		public DatabaseInitializer(
			ApplicationDbContext applicationContext,
			ILogger<DatabaseInitializer> logger)
		{
			_applicationContext = applicationContext;
			_logger = logger;
		}

		public async Task SeedAsync()
		{
			await _applicationContext.Database.MigrateAsync().ConfigureAwait(false);

			/*
				Package Manager Console:
				add-migration InitialIdentityMigration -Context IdentityDbContext
				add-migration InitialApplicationMigration -Context ApplicationDbContext
			*/

			//Seems like this needs an update from Core
			//this is a workaround for fast standups as Migrate() should create this
			//_applicationContext.Database.EnsureCreated();

			if (!await _applicationContext.Errors.AnyAsync())
			{
				for (int i = 0; i < dataPopulationCount; i++)
					try
					{
						ErrorFactory.ThrowException();
					}
					catch (Exception exception)
					{
						Error error = ErrorFactory.GetErrorFromException(
							exception,
							LogLevel.Information,
							"Expected seeded error.");
						error.CreatedDate = DateTime.UtcNow;
						error.CreatedBy = "Test";

						_applicationContext.Errors.Add(error);
					}
				_applicationContext.SaveChanges();

				_logger.LogInformation("Application Error generation completed");
			}


			_logger.LogInformation("Seeding initial data completed");
		}
	}
}