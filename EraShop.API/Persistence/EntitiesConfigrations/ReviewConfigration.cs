using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class ReviewConfigration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.Property(r => r.Comment)
				.HasMaxLength(500);

			builder.HasOne(r => r.Product)
				.WithMany()
				.HasForeignKey(r => r.ProductId);

			builder.HasOne(r => r.User)
				.WithMany()
				.HasForeignKey(r => r.UserId);
		}
	}
}
