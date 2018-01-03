namespace Jt76.Data
{
	using Microsoft.AspNetCore.Http;
	using AspNet.Security.OpenIdConnect.Primitives;
	using DbContexts;

	public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(
			Jt76ApplicationDbContext applicationContext,
			Jt76IdentityDbContext identityContext, 
			IHttpContextAccessor httpAccessor) : base(applicationContext, identityContext)
        {
	        identityContext.CurrentUserId = 
				httpAccessor.HttpContext.User
				.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }
    }
}
