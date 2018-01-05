namespace Jt76.Identity.Policies
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Common.Constants;
	using Microsoft.AspNetCore.Authorization;
	using IdentityHelper = Helpers.IdentityHelper;

	public class ViewUserByIdRequirement : IAuthorizationRequirement
	{

	}


	public class ViewUserByIdHandler : AuthorizationHandler<ViewUserByIdRequirement, string>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewUserByIdRequirement requirement, string targetUserId)
		{
			if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers) || GetIsSameUser(context.User, targetUserId))
				context.Succeed(requirement);

			return Task.CompletedTask;
		}


		private bool GetIsSameUser(ClaimsPrincipal user, string targetUserId)
		{
			return IdentityHelper.GetUserId(user) == targetUserId;
		}
	}
}
