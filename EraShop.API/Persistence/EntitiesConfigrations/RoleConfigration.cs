using EraShop.API.Abstractions.Consts;
using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class RoleConfigration : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			//Default Data
			builder.HasData([
				new ApplicationRole
				{
					Id = DefaultRoles.AdminRoleId,
					Name = DefaultRoles.Admin,
					NormalizedName = DefaultRoles.Admin.ToUpper(),
					ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
				},
				new ApplicationRole
				{
					Id = DefaultRoles.UserRoleId,
					Name = DefaultRoles.User,
					NormalizedName = DefaultRoles.User.ToUpper(),
					ConcurrencyStamp = DefaultRoles.UserRoleConcurrencyStamp,
					IsDefault = true
				},
				new ApplicationRole
				{
					Id = DefaultRoles.SellerRoleId,
					Name = DefaultRoles.Seller,
					NormalizedName = DefaultRoles.Seller.ToUpper(),
					ConcurrencyStamp = DefaultRoles.SellerRoleConcurrencyStamp,
					IsDefault = true
				}
			]);
		}
	}
}
