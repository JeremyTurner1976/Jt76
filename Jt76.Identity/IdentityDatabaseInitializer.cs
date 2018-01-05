namespace Jt76.Identity
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Common.CommonData.Interfaces;
	using Common.Constants;
	using DbContexts;
	using Interfaces;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;
	using Models;

	public class IdentityDatabaseInitializer : IDatabaseInitializer
	{
		private readonly IdentityDbContext _identityContext;
		private readonly IAccountManager _accountManager;
		private readonly ILogger _logger;

		public IdentityDatabaseInitializer(
			IdentityDbContext IdentityContext,
			IAccountManager accountManager, 
			ILogger<IdentityDatabaseInitializer> logger)
		{
			_accountManager = accountManager;
			_identityContext = IdentityContext;
			_logger = logger;
		}

		public async Task SeedAsync()
		{
			//Seems like this needs an update from Core
			//this is a workaround as Migrate should create this
			//await _identityContext.Database.MigrateAsync().ConfigureAwait(false);
			_identityContext.Database.EnsureCreated();

			if (!await _identityContext.Users.AnyAsync())
			{
				_logger.LogInformation("Generating built in accounts");

				const string adminRoleName = "administrator";
				const string userRoleName = "user";

				await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
				await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

				await CreateUserAsync("admin", "tempP@ss123", "Built In Administrator", "admin@test.com",
					"+1 (123) 000-0000", new string[] {adminRoleName});
				await CreateUserAsync("user", "tempP@ss123", "Built In Standard User", "user@test.com", "+1 (123) 000-0001",
					new string[] {userRoleName});

				_logger.LogInformation("Identity User account generation completed");
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