namespace Jt76.Data
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Common.Constants;
	using DbContexts;
	using Factories;
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;
	using Models.ApplicationDb;
	using Models.IdentityDb;

	public class DatabaseInitializer : IDatabaseInitializer
	{
		private readonly Jt76ApplicationDbContext _applicationContext;
		private readonly Jt76IdentityDbContext _identityContext;
		private readonly IAccountManager _accountManager;
		private readonly ILogger _logger;
		private const int dataPopulationCount = 25;

		public DatabaseInitializer(
			Jt76ApplicationDbContext applicationContext, 
			Jt76IdentityDbContext identityContext,
			IAccountManager accountManager, 
			ILogger<DatabaseInitializer> logger)
		{
			_accountManager = accountManager;
			_applicationContext = applicationContext;
			_identityContext = identityContext;
			_logger = logger;
		}

		public async Task SeedAsync()
		{
			await _identityContext.Database.MigrateAsync().ConfigureAwait(false);

			if (!await _identityContext.Users.AnyAsync())
			{
				_logger.LogInformation("Generating inbuilt accounts");

				const string adminRoleName = "administrator";
				const string userRoleName = "user";

				await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
				await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

				await CreateUserAsync("admin", "tempP@ss123", "Inbuilt Administrator", "admin@ebenmonney.com",
					"+1 (123) 000-0000", new string[] {adminRoleName});
				await CreateUserAsync("user", "tempP@ss123", "Inbuilt Standard User", "user@ebenmonney.com", "+1 (123) 000-0001",
					new string[] {userRoleName});

				_logger.LogInformation("User account generation completed");
			}

			await _applicationContext.Database.MigrateAsync().ConfigureAwait(false);

			if (!await _applicationContext.Errors.AnyAsync())
			{
				for (int i = 0; i < dataPopulationCount; i++)
				{
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
				}
				_applicationContext.SaveChanges();
			}


			_logger.LogInformation("Seeding initial data completed");
		}

		private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
		{
			if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
			{
				ApplicationRole applicationRole = new ApplicationRole(roleName, description);

				var result = await _accountManager.CreateRoleAsync(applicationRole, claims);

				if (!result.Item1)
					throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
			}
		}

		private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
		{
			ApplicationUser applicationUser = new ApplicationUser
			{
				UserName = userName,
				FullName = fullName,
				Email = email,
				PhoneNumber = phoneNumber,
				EmailConfirmed = true,
				IsEnabled = true
			};

			var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

			if (!result.Item1)
				throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");


			return applicationUser;
		}
	}
}