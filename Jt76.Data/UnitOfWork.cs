namespace Jt76.Data
{
	using DbContexts;
	using Interfaces;
	using Repositories;
	using Repositories.Interfaces;

	public class UnitOfWork : IUnitOfWork
    {
	    readonly Jt76ApplicationDbContext _jt76ApplicationDbContext;
		readonly Jt76IdentityDbContext _jt76IdentityDbContext;

		private IErrorRepository _errors;

        public UnitOfWork(
	        Jt76ApplicationDbContext jt76ApplicationDbContext,
			Jt76IdentityDbContext jt76IdentityDbContext)
        {
	        _jt76ApplicationDbContext = jt76ApplicationDbContext;
	        _jt76IdentityDbContext = jt76IdentityDbContext;

        }

        public IErrorRepository Errors
        {
            get
            {
                if (_errors == null)
	                _errors = new ErrorRepository(_jt76ApplicationDbContext);

                return _errors;
            }
        }

	    public int SaveChanges()
	    {
		    int totalSaves;
		    totalSaves = _jt76ApplicationDbContext.SaveChanges();
		    totalSaves += _jt76IdentityDbContext.SaveChanges();
			return totalSaves;
	    }

	    public int SaveAppChanges()
        {
            return _jt76ApplicationDbContext.SaveChanges();
        }

	    public int SaveIdentityChanges()
	    {
		    return _jt76IdentityDbContext.SaveChanges();
	    }
    }
}
