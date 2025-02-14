using EraShop.API.Abstractions;
using EraShop.API.Contracts.Baskets;
using EraShop.API.Contracts.Orders;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using EraShop.API.Settings;
using Mapster;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Stripe;
using System.Text.Json;

namespace EraShop.API.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IBasketService _basketService;
		private readonly IDatabase _database;
		private readonly IProductService _productService;
		private readonly StripeSettings _stripeSettings;
		private readonly ApplicationDbContext _context;
		private readonly ILogger<AuthService> _logger;
		public PaymentService(IBasketService basketService,
			IProductService productService,
			IConnectionMultiplexer redis,
			ApplicationDbContext context,
			IOptions<StripeSettings> stripeSettings,
			ILogger<AuthService> logger)
		{
			_basketService = basketService;
			_context = context;
			_productService = productService;
			_database = redis.GetDatabase();
			_logger = logger;
			_stripeSettings = stripeSettings.Value;
		}
		public async Task<Result<CustomerBasketResponse>> CreateOrUpdatePaymentIntent(string basketId)
		{
			StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
			var basketResult = await _basketService.GetCustomerBasketAsync(basketId);

			if (basketResult.IsFailure)
				return Result.Failure<CustomerBasketResponse>(BasketErrors.BasketNotFound);

			var basket = basketResult.Value;

			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethode = await _context.DeliveryMethods.FirstOrDefaultAsync(d => d.Id == basket.DeliveryMethodId.Value);
				if (deliveryMethode is null)
					return Result.Failure<CustomerBasketResponse>(BasketErrors.BasketNotFound);

				// aaa
				basket = basket with { ShippingPrice = deliveryMethode.Cost };
			}

			if (basket.Items.Count() > 0)
			{
				var updatedItems = new List<BasketItemResponse>();
				foreach (var item in basket.Items)
				{
					var productResult = await _productService.GetByIdAsync(item.Id);
					if (productResult.IsFailure)
						return Result.Failure<CustomerBasketResponse>(ProductErrors.ProductNotFound);

					var product = productResult.Value;

					if (item.Price != product.Price)
						updatedItems.Add(item with { Price = product.Price });
					else
						updatedItems.Add(item);
				}
				basket = basket with { Items = updatedItems };
			}
			PaymentIntent? paymentIntent = null;
			PaymentIntentService paymentIntentService = new PaymentIntentService();

			if (string.IsNullOrEmpty(basket.PaymentIntentId))
			{

				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)basket.Items.Sum(items => items.Price * 100 * items.Quantity) + (long)basket.ShippingPrice * 100,
					Currency = "USD",
					PaymentMethodTypes = new List<string>() { "card" }
				};

				paymentIntent = await paymentIntentService.CreateAsync(options);
				basket = basket with
				{
					PaymentIntentId = paymentIntent.Id,
					ClientSecret = paymentIntent.ClientSecret
				};

			}
			else
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)basket.Items.Sum(items => items.Price * 100 * items.Quantity) + (long)basket.ShippingPrice * 100,
				};

				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);

			}
			var value = JsonSerializer.Serialize(basket);
			var updated = await _database.StringSetAsync(basket.Id, value, TimeSpan.FromDays(15));
			return Result.Success(basket);
		}

		public async Task<Result> UpdateOrderPaymentStatus(string requestBody, string header)
		{
			var stripeEvent = EventUtility.ConstructEvent(requestBody, header, _stripeSettings.WebhookSecret);
			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
			Result<OrderResponse> orderResult;
			switch (stripeEvent.Type)
			{
				case "payment_intent.succeeded":
					orderResult = await UpdatePaymentIntent(paymentIntent.Id, isPaid: true);
					_logger.LogInformation("Order is Succeeded With Payment IntentId:{0}", paymentIntent.Id);
					break;
				case "payment_intent.payment_failed":
					orderResult = await UpdatePaymentIntent(paymentIntent.Id, isPaid: false);
					_logger.LogInformation("Order is !Succeeded With Payment IntentId:{0}", paymentIntent.Id);
					break;
				default:
					return Result.Failure(OrderErrors.OrderNotFound);
			}
			if (orderResult.IsFailure)
				return Result.Failure(orderResult.Error);

			return Result.Success();
		}
		private async Task<Result<OrderResponse>> UpdatePaymentIntent(string paymentIntentId, bool isPaid)
		{
			var order = await _context.Orders
							.Include(o => o.ShippingAddress)
							.Include(o => o.DeliveryMethod)
							.FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);

			if (order is null)
				return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);

			if (isPaid)
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaymentFailed;

			_context.Orders.Update(order);
			_context.SaveChanges();
			return Result.Success(order.Adapt<OrderResponse>());

		}
	}
}
