﻿namespace Jt76.Identity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Common.Constants;
	using DbContexts;
	using Interfaces;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Models;

	public class AccountManager : IAccountManager
	{
		private readonly IdentityDbContext _context;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountManager(IdentityDbContext context, UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<ApplicationUser> GetUserByIdAsync(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}

		public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
		{
			return await _userManager.FindByNameAsync(userName);
		}

		public async Task<ApplicationUser> GetUserByEmailAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}

		public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
		{
			return await _userManager.GetRolesAsync(user);
		}


		public async Task<Tuple<ApplicationUser, string[]>> GetUserAndRolesAsync(string userId)
		{
			ApplicationUser user = await _context.Users
				.Include(u => u.Roles)
				.Where(u => u.Id == userId)
				.FirstOrDefaultAsync();

			if (user == null)
				return null;

			List<string> userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

			string[] roles = await _context.Roles
				.Where(r => userRoleIds.Contains(r.Id))
				.Select(r => r.Name)
				.ToArrayAsync();

			return Tuple.Create(user, roles);
		}


		public async Task<List<Tuple<ApplicationUser, string[]>>> GetUsersAndRolesAsync(int page, int pageSize)
		{
			IQueryable<ApplicationUser> usersQuery = _context.Users
				.Include(u => u.Roles)
				.OrderBy(u => u.UserName);

			if (page != -1)
				usersQuery = usersQuery.Skip((page - 1) * pageSize);

			if (pageSize != -1)
				usersQuery = usersQuery.Take(pageSize);

			List<ApplicationUser> users = await usersQuery.ToListAsync();

			List<string> userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

			ApplicationRole[] roles = await _context.Roles
				.Where(r => userRoleIds.Contains(r.Id))
				.ToArrayAsync();

			return users.Select(u => Tuple.Create(u,
					roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToArray()))
				.ToList();
		}


		public async Task<Tuple<bool, string[]>> CreateUserAsync(ApplicationUser user, IEnumerable<string> roles,
			string password)
		{
			IdentityResult result = await _userManager.CreateAsync(user, password);
			if (!result.Succeeded)
				return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

			user = await _userManager.FindByNameAsync(user.UserName);

			try
			{
				result = await _userManager.AddToRolesAsync(user, roles.Distinct());
			}
			catch
			{
				await DeleteUserAsync(user);
				throw;
			}

			if (result.Succeeded) return Tuple.Create(true, new string[] { });

			await DeleteUserAsync(user);
			return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
		}


		public async Task<Tuple<bool, string[]>> UpdateUserAsync(ApplicationUser user)
		{
			return await UpdateUserAsync(user, null);
		}


		public async Task<Tuple<bool, string[]>> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
		{
			IdentityResult result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
				return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

			if (roles == null) return Tuple.Create(true, new string[] { });

			IList<string> userRoles = await _userManager.GetRolesAsync(user);

			IEnumerable<string> enumerable = roles as string[] ?? roles.ToArray();
			string[] rolesToRemove = userRoles.Except(enumerable).ToArray();
			string[] rolesToAdd = enumerable.Except(userRoles).Distinct().ToArray();

			if (rolesToRemove.Any())
			{
				result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
				if (!result.Succeeded)
					return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
			}

			if (!rolesToAdd.Any()) return Tuple.Create(true, new string[] { });

			result = await _userManager.AddToRolesAsync(user, rolesToAdd);
			return result.Succeeded
				? Tuple.Create(true, new string[] { })
				: Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
		}


		public async Task<Tuple<bool, string[]>> ResetPasswordAsync(ApplicationUser user, string newPassword)
		{
			string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

			IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
			return result.Succeeded
				? Tuple.Create(true, new string[] { })
				: Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
		}

		public async Task<Tuple<bool, string[]>> UpdatePasswordAsync(ApplicationUser user, string currentPassword,
			string newPassword)
		{
			IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
			return result.Succeeded
				? Tuple.Create(true, new string[] { })
				: Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
		}

		public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
		{
			if (await _userManager.CheckPasswordAsync(user, password)) return true;

			if (!_userManager.SupportsUserLockout)
				await _userManager.AccessFailedAsync(user);

			return false;
		}


		public async Task<bool> TestCanDeleteUserAsync(string userId)
		{
			//Handle Foreign Key Lookups
			//canDelete = !await ; //Do other tests...
			await GetUserAndRolesAsync(userId);

			return true;
		}

		public async Task<Tuple<bool, string[]>> DeleteUserAsync(string userId)
		{
			ApplicationUser user = await _userManager.FindByIdAsync(userId);

			if (user != null)
				return await DeleteUserAsync(user);

			return Tuple.Create(true, new string[] { });
		}


		public async Task<Tuple<bool, string[]>> DeleteUserAsync(ApplicationUser user)
		{
			IdentityResult result = await _userManager.DeleteAsync(user);
			return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
		}


		public async Task<ApplicationRole> GetRoleByIdAsync(string roleId)
		{
			return await _roleManager.FindByIdAsync(roleId);
		}


		public async Task<ApplicationRole> GetRoleByNameAsync(string roleName)
		{
			return await _roleManager.FindByNameAsync(roleName);
		}


		public async Task<ApplicationRole> GetRoleLoadRelatedAsync(string roleName)
		{
			ApplicationRole role = await _context.Roles
				.Include(r => r.Claims)
				.Include(r => r.Users)
				.Where(r => r.Name == roleName)
				.FirstOrDefaultAsync();

			return role;
		}


		public async Task<List<ApplicationRole>> GetRolesLoadRelatedAsync(int page, int pageSize)
		{
			IQueryable<ApplicationRole> rolesQuery = _context.Roles
				.Include(r => r.Claims)
				.Include(r => r.Users)
				.OrderBy(r => r.Name);

			if (page != -1)
				rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

			if (pageSize != -1)
				rolesQuery = rolesQuery.Take(pageSize);

			List<ApplicationRole> roles = await rolesQuery.ToListAsync();

			return roles;
		}


		public async Task<Tuple<bool, string[]>> CreateRoleAsync(ApplicationRole role, IEnumerable<string> claims)
		{
			if (claims == null)
				claims = new string[] { };

			IEnumerable<string> enumerable = claims as string[] ?? claims.ToArray();
			string[] invalidClaims = enumerable.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
			if (invalidClaims.Any())
				return Tuple.Create(false, new[] {"The following claim types are invalid: " + string.Join(", ", invalidClaims)});


			IdentityResult result = await _roleManager.CreateAsync(role);
			if (!result.Succeeded)
				return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


			role = await _roleManager.FindByNameAsync(role.Name);

			foreach (string claim in enumerable.Distinct())
			{
				result = await _roleManager.AddClaimAsync(role,
					new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

				if (result.Succeeded) continue;

				await DeleteRoleAsync(role);
				return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
			}

			return Tuple.Create(true, new string[] { });
		}

		public async Task<Tuple<bool, string[]>> UpdateRoleAsync(ApplicationRole role, IEnumerable<string> claims)
		{
			IEnumerable<string> second = claims as string[] ?? claims.ToArray();
			{
				string[] invalidClaims = second.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
				if (invalidClaims.Any())
					return Tuple.Create(false, new[] {"The following claim types are invalid: " + string.Join(", ", invalidClaims)});
			}


			IdentityResult result = await _roleManager.UpdateAsync(role);
			if (!result.Succeeded)
				return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

			{
				IEnumerable<Claim> roleClaims =
					(await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == CustomClaimTypes.Permission);
				IEnumerable<Claim> enumerable = roleClaims as Claim[] ?? roleClaims.ToArray();
				string[] roleClaimValues = enumerable.Select(c => c.Value).ToArray();

				string[] claimsToRemove = roleClaimValues.Except(second).ToArray();
				string[] claimsToAdd = second.Except(roleClaimValues).Distinct().ToArray();

				if (claimsToRemove.Any())
					foreach (string claim in claimsToRemove)
					{
						result = await _roleManager.RemoveClaimAsync(role, enumerable.FirstOrDefault(c => c.Value == claim));
						if (!result.Succeeded)
							return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
					}

				if (!claimsToAdd.Any()) return Tuple.Create(true, new string[] { });

				foreach (string claim in claimsToAdd)
				{
					result = await _roleManager.AddClaimAsync(role,
						new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
					if (!result.Succeeded)
						return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
				}
			}

			return Tuple.Create(true, new string[] { });
		}


		public async Task<bool> TestCanDeleteRoleAsync(string roleId)
		{
			return !await _context.UserRoles.Where(r => r.RoleId == roleId).AnyAsync();
		}


		public async Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName)
		{
			ApplicationRole role = await _roleManager.FindByNameAsync(roleName);

			if (role != null)
				return await DeleteRoleAsync(role);

			return Tuple.Create(true, new string[] { });
		}


		public async Task<Tuple<bool, string[]>> DeleteRoleAsync(ApplicationRole role)
		{
			IdentityResult result = await _roleManager.DeleteAsync(role);
			return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
		}
	}
}