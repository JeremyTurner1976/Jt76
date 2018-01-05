namespace Jt76.Data
{
	using Common.CommonData.Interfaces;
	using DbContexts;
	using Repositories;
	using Repositories.Interfaces;

	public class UnitOfWork : IUnitOfWork
	{
		readonly ApplicationDbContext _applicationDbContext;

		private IErrorRepository _errors;

		public UnitOfWork(
			ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;

		}

		public IErrorRepository Errors
		{
			get
			{
				if (_errors == null)
					_errors = new ErrorRepository(_applicationDbContext);

				return _errors;
			}
		}

		public int SaveChanges()
		{
			return _applicationDbContext.SaveChanges();
		}
	}
}
