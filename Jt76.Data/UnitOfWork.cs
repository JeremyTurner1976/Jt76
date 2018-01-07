namespace Jt76.Data
{
	using Common.CommonData.Interfaces;
	using DbContexts;
	using Repositories;
	using Repositories.Interfaces;

	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _applicationDbContext;

		private IErrorRepository _errors;

		public UnitOfWork(
			ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public IErrorRepository Errors =>
			_errors ?? (_errors = new ErrorRepository(_applicationDbContext));

		public int SaveChanges()
		{
			return _applicationDbContext.SaveChanges();
		}
	}
}