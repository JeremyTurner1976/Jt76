namespace Jt76.Data.DbContexts
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Interfaces;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Models.IdentityDb;

	public class Jt76IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		public string CurrentUserId { get; set; }

		public Jt76IdentityDbContext(DbContextOptions<Jt76IdentityDbContext> options) : base(options)
		{ }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
			builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

			builder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
			builder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
		}




		public override int SaveChanges()
		{
			UpdateAuditEntities();
			return base.SaveChanges();
		}


		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			UpdateAuditEntities();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}


		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			UpdateAuditEntities();
			return base.SaveChangesAsync(cancellationToken);
		}


		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			UpdateAuditEntities();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}


		private void UpdateAuditEntities()
		{
			var modifiedEntries = ChangeTracker.Entries()
				.Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


			foreach (var entry in modifiedEntries)
			{
				var entity = (IAuditableEntity)entry.Entity;
				DateTime now = DateTime.UtcNow;

				if (entry.State == EntityState.Added)
				{
					entity.CreatedDate = now;
					entity.CreatedBy = CurrentUserId;
				}
				else
				{
					Entry(entity).Property(x => x.CreatedBy).IsModified = false;
					Entry(entity).Property(x => x.CreatedDate).IsModified = false;
				}

				entity.UpdatedDate = now;
				entity.UpdatedBy = CurrentUserId;
			}
		}
	}
}
