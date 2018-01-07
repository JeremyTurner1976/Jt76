namespace Jt76.Identity
{
	using AspNet.Security.OpenIdConnect.Primitives;
	using DbContexts;
	using Microsoft.AspNetCore.Http;

	public class HttpUnitOfWork : IdentityUnitOfWork
	{
		public HttpUnitOfWork(
			IdentityDbContext IdentityContext,
			IHttpContextAccessor httpAccessor) : base(IdentityContext)
		{
			IdentityContext.CurrentUserId =
				httpAccessor.HttpContext.User
					.FindFirst(OpenIdConnectConstants.Claims.Subject)
					?.Value?.Trim();
		}
	}
}