using EraShop.API.Abstractions;
using EraShop.API.Contracts.Baskets;

namespace EraShop.API.Services
{
	public interface IPaymentService
	{
		public Task<Result<CustomerBasketResponse>> CreateOrUpdatePaymentIntent(string basketId);
		public Task<Result> UpdateOrderPaymentStatus(string requestBody, string header);
	}
}
