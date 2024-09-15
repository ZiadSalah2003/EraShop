using EraShop.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EraShop.API.Persistence
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(modelBuilder);
		}
	}
}
