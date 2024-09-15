using EraShop.API.Abstractions.Consts;
using EraShop.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class UserConfigration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			//Default Data
			var passwordHasher = new PasswordHasher<ApplicationUser>();

			builder.HasData(new ApplicationUser
			{
				Id = DefaultUsers.AdminId,
				FirstName = "Era Shop",
				LastName = "Admin",
				UserName = DefaultUsers.AdminEmail,
				NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
				Email = DefaultUsers.AdminEmail,
				NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
				SecurityStamp = DefaultUsers.AdminSecurityStamp,
				ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
				EmailConfirmed = true,
				PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
			});
		}
	}
}
