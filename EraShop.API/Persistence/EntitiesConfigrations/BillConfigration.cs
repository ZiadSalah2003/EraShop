using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class BillConfigration : IEntityTypeConfiguration<Bill>
	{
		public void Configure(EntityTypeBuilder<Bill> builder)
		{
			builder.Property(order => order.Subtotal).HasColumnType("decimal(8,2)");
			builder.Property(order => order.Status)
				.HasConversion
				(
					(OStatus) => OStatus.ToString(),
					(OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
				);
		}
	}
}
