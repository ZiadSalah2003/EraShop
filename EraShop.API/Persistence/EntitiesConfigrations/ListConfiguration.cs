using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
    public class ListConfiguration : IEntityTypeConfiguration<List>
    {
        public void Configure(EntityTypeBuilder<List> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => new { x.Name, x.UserId }).IsUnique();

        }
    }
}
