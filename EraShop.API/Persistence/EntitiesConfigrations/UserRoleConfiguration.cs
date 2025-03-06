using EraShop.API.Abstractions.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            //Default Data
            builder.HasData
            (
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.AdminId,
                    RoleId = DefaultRoles.AdminRoleId
                }
            );
        }
    }
}
