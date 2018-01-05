namespace Jt76.Identity
{
	using Common.CommonData.Interfaces;
	using DbContexts;

	public class IdentityUnitOfWork : IUnitOfWork
	{
		readonly IdentityDbContext _IdentityDbContext;

		public IdentityUnitOfWork(
			IdentityDbContext IdentityDbContext)
		{
			_IdentityDbContext = IdentityDbContext;

		}

		public int SaveChanges()
		{
			return _IdentityDbContext.SaveChanges();
		}
	}
}
