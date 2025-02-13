using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
    public class ListItemConfiguration : IEntityTypeConfiguration<ListItem>
    {
        public void Configure(EntityTypeBuilder<ListItem> builder)
        {
            builder.HasIndex(x => new { x.ListId, x.ProductId }).IsUnique();

            builder.HasOne(li => li.List)
             .WithMany(l => l.Items)
             .HasForeignKey(li => li.ListId)
             .OnDelete(DeleteBehavior.Cascade); // Explicitly set Cascade
        }
    }
}
