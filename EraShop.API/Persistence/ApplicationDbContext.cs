using EraShop.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace EraShop.API.Persistence
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			var cascadeFKs = modelBuilder.Model
				.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

			foreach (var fk in cascadeFKs)
				fk.DeleteBehavior = DeleteBehavior.Restrict;

			base.OnModelCreating(modelBuilder);
		}
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Review> Reviews { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditableEntity>();
			foreach (var entityEntry in entries)
			{
				var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
				if (entityEntry.State == EntityState.Added)
				{
					entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
				}
				if (entityEntry.State == EntityState.Modified)
				{
					entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
					entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
				}

			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
