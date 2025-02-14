using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, shippingaddres => shippingaddres.WithOwner());
			builder.Property(order => order.Status)
				.HasConversion
				(
					(OStatus) => OStatus.ToString(),
					(OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
				);
			builder.Property(order => order.Subtotal).HasColumnType("decimal(8,2)");
			builder.HasOne(order => order.DeliveryMethod)
				.WithMany()
				.HasForeignKey(order => order.DeliveryMethodId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasMany(order => order.Items)
				.WithOne()
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
