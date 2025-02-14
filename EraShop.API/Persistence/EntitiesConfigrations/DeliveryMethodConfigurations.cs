using EraShop.API.Abstractions.Consts;
using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
	{
		public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
		{
			builder.Property(method => method.Cost)
				.HasColumnType("decimal(8,2)");

			builder.HasData([
				new DeliveryMethod
				{
					Id = DefaultDeliveryMethod.Id,
					ShortName = DefaultDeliveryMethod.ShortName,
					Description = DefaultDeliveryMethod.Description,
					Cost = DefaultDeliveryMethod.Cost,
					DeliveryTime = DefaultDeliveryMethod.DeliveryTime
				},
				new DeliveryMethod
				{
					Id=DefaultDeliveryMethod.Id1,
					ShortName =DefaultDeliveryMethod.ShortName1,
					Description = DefaultDeliveryMethod.Description1,
					Cost = DefaultDeliveryMethod.Cost1,
					DeliveryTime = DefaultDeliveryMethod.DeliveryTime1
				},
				new DeliveryMethod
				{
					Id=DefaultDeliveryMethod.Id2,
					ShortName =DefaultDeliveryMethod.ShortName2,
					Description = DefaultDeliveryMethod.Description2,
					Cost = DefaultDeliveryMethod.Cost2,
					DeliveryTime = DefaultDeliveryMethod.DeliveryTime2
				},
				new DeliveryMethod
				{
					Id=DefaultDeliveryMethod.Id3,
					ShortName =DefaultDeliveryMethod.ShortName3,
					Description = DefaultDeliveryMethod.Description3,
					Cost = DefaultDeliveryMethod.Cost3,
					DeliveryTime = DefaultDeliveryMethod.DeliveryTime3
				}
			]);
		}
	}
}
