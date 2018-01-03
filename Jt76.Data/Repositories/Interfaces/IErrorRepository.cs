namespace Jt76.Data.Repositories.Interfaces
{
	using System.Collections.Generic;
	using Data.Interfaces;
	using Models.ApplicationDb;

	public interface IErrorRepository : IRepository<Error>
	{
		IEnumerable<Error> GetTopErrors(int count);
	}
}
