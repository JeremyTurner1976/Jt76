namespace Jt76.Data.Interfaces
{
	using Repositories.Interfaces;

	public interface IUnitOfWork
    {
        //IErrorRepository Errors { get; }

        int SaveChanges();
    }
}
