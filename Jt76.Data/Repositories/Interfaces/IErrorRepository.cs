namespace Jt76.Data.Repositories.Interfaces
{
	using System.Collections.Generic;
	using Common.CommonData.Interfaces;
	using Models;

	public interface IErrorRepository : IRepository<Error>
	{
		IEnumerable<Error> GetTopErrors(int count);
	}
}