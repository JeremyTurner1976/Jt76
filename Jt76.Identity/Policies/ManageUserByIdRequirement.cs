﻿namespace Jt76.Identity.Policies
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Common.Constants;
	using Helpers;
	using Microsoft.AspNetCore.Authorization;

	public class ManageUserByIdRequirement : IAuthorizationRequirement
	{
	}


	public class ManageUserByIdHandler : AuthorizationHandler<ManageUserByIdRequirement, string>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			ManageUserByIdRequirement requirement, string userId)
		{
			if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers) ||
			    GetIsSameUser(context.User, userId))
				context.Succeed(requirement);

			return Task.CompletedTask;
		}


		private bool GetIsSameUser(ClaimsPrincipal user, string userId)
		{
			return IdentityHelper.GetUserId(user) == userId;
		}
	}
}