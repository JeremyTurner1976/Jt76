namespace Jt76.Data.DbContexts
{
	using Microsoft.EntityFrameworkCore;
	using Models;

	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Error> Errors { get; set; }
	}
}