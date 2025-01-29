using EraShop.API.Abstractions;
using EraShop.API.Contracts.Baskets;
using EraShop.API.Errors;
using Mapster;
using Org.BouncyCastle.Bcpg;
using StackExchange.Redis;
using System.Text.Json;

namespace EraShop.API.Services
{
	public class BasketService : IBasketService
	{
		private readonly IDatabase _database;
		public BasketService(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}
		public async Task<Result<CustomerBasketResponse>> GetCustomerBasketAsync(string id)
		{
			var basket = await _database.StringGetAsync(id);
			if(basket.IsNullOrEmpty)
				return Result.Failure<CustomerBasketResponse>(BasketErrors.BasketNotFound);

			var response = JsonSerializer.Deserialize<CustomerBasketResponse>(basket);
			return Result.Success(response!);
		}

		public async Task<Result<CustomerBasketResponse>> UpdateCustomerBasketAsync(CustomerBasketRequest request)
		{


			var value = JsonSerializer.Serialize(request);
			var updated = await _database.StringSetAsync(request.Id, value, TimeSpan.FromDays(15));
			if (!updated)
				return Result.Failure<CustomerBasketResponse>(BasketErrors.BasketUpdateFailed);

			var response = request.Adapt<CustomerBasketResponse>();
			return Result.Success(response);
		}

		public async Task<Result> DeleteCustomerBasketAsync(string id)
		{
			var deleted = await _database.KeyDeleteAsync(id);

			if (!deleted)
				return Result.Failure<CustomerBasketResponse>(BasketErrors.BasketNotFound);

			return Result.Success();
		}
	}
}
