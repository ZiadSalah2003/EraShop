using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.OwnsOne(item => item.Product, product => product.WithOwner());
			builder.Property(item => item.Price).HasColumnType("decimal(8,2)");
		}
	}
}
