using EraShop.API.Contracts.Orders;

namespace EraShop.API.Services
{
	public interface IOrderService
	{
		public Task<Result<OrderResponse>> CreateOrderAsync(string buyerEmail, OrderCreateRequest request);
		public Task<Result<IEnumerable<OrderResponse>>> GetOrdersForUserAsync(string buyerEmail);
		public Task<Result<OrderResponse>> GetOrderByIdAsync(string buyerEmail, int orderId);
		public Task<Result<IEnumerable<DeliveryMethodResponse>>> GetDeliveryMethodsAsync();
	}
}
