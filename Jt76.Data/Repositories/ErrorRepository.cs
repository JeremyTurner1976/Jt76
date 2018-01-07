namespace Jt76.Data.Repositories
{
	using System.Collections.Generic;
	using System.Linq;
	using Common.CommonData;
	using DbContexts;
	using Interfaces;
	using Models;

	public class ErrorRepository : Repository<Error>, IErrorRepository
	{
		public ErrorRepository(ApplicationDbContext context) : base(context)
		{
		}

		private ApplicationDbContext appContext => (ApplicationDbContext) _context;

		public IEnumerable<Error> GetTopErrors(int count)
		{
			return appContext.Errors
				.Take(count)
				.OrderBy(x => x.UpdatedDate)
				.ThenBy(x => x.ErrorLevel)
				.ToList();
		}
	}
}