namespace Jt76.Data.DbContexts
{
	using Microsoft.EntityFrameworkCore;
	using Models.ApplicationDb;

	public class Jt76ApplicationDbContext : DbContext
	{
		public Jt76ApplicationDbContext(DbContextOptions<Jt76ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Error> Errors { get; set; }
	}
}