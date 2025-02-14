using EraShop.API.Contracts.Authentication;
using EraShop.API.Contracts.Orders;
using EraShop.API.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;

namespace EraShop.API.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SignUpRequest, ApplicationUser>()
                .Map(dest => dest.UserName, dest => dest.Email);

			config.NewConfig<OrderItem, OrderItemResponse>()
				.Map(dest => dest.ProductId, src => src.Product.ProductId)
				.Map(dest => dest.ProductName, src => src.Product.ProductName)
				.Map(dest => dest.PictureUrl, src => src.Product.PictureUrl);

			config.NewConfig<Order, OrderResponse>()
				.Map(dest => dest.DeliveryMethodId, src => src.DeliveryMethod.Id)
				.Map(dest => dest.DeliveryMethod, src => src.DeliveryMethod.ShortName);

		}
	}
}
