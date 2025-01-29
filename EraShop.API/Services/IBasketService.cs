using EraShop.API.Abstractions;
using EraShop.API.Contracts.Baskets;

namespace EraShop.API.Services
{
	public interface IBasketService
	{
		public Task<Result<CustomerBasketResponse>> GetCustomerBasketAsync(string id);
		public Task<Result<CustomerBasketResponse>> UpdateCustomerBasketAsync(CustomerBasketRequest request);
		public Task<Result> DeleteCustomerBasketAsync(string id);
	}
}
