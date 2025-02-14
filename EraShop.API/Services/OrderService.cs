using EraShop.API.Abstractions;
using EraShop.API.Contracts.Orders;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;

namespace EraShop.API.Services
{
	public class OrderService : IOrderService
	{
		private readonly IBasketService _basketService;
		private readonly IProductService _productService;
		private readonly IPaymentService _paymentService;
		private readonly ApplicationDbContext _context;
		public OrderService(IProductService productService, IPaymentService paymentService, IBasketService basketService, ApplicationDbContext context)
		{
			_basketService = basketService;
			_productService = productService;
			_context = context;
			_paymentService = paymentService;
		}
		public async Task<Result<OrderResponse>> CreateOrderAsync(string buyerEmail, OrderCreateRequest request)
		{
			if(buyerEmail is null)
				return Result.Failure<OrderResponse>(UserErrors.UserEmailNotFound);

			var basketResult = await _basketService.GetCustomerBasketAsync(request.BasketId);
			if (!basketResult.IsSuccess)
				return Result.Failure<OrderResponse>(OrderErrors.BasketRetriveError);

			var basket = basketResult.Value;

			if(basket.PaymentIntentId is null)
				return Result.Failure<OrderResponse>(OrderErrors.PaymentIntentNotFound);
			var orderitems = new List<OrderItem>();
			if (basket.Items.Count() > 0)
			{
				foreach (var item in basket.Items)
				{
					var productResult = await _productService.GetByIdAsync(item.Id);
					var product = productResult.Value;
					if (product.Name is null)
						return Result.Failure<OrderResponse>(ProductErrors.ProductNotFound);

					if (productResult.IsSuccess)
					{
						var productItemOrdered = new ProductItemOrdered()
						{
							ProductId = product.Id,
							ProductName = product.Name,
							PictureUrl = product.ImageUrl ?? "",
						};
						var orderitem = new OrderItem()
						{
							Product = productItemOrdered,
							Price = product.Price,
							Quantity = item.Quantity,
						};
						orderitems.Add(orderitem);
					}
				}
			}
			
			var subtotal = orderitems.Sum(item => item.Price * item.Quantity);
			var address = new Address
			{
				FirstName = request.ShipToAddress.FirstName,
				LastName = request.ShipToAddress.LastName,
				Street = request.ShipToAddress.Street,
				City = request.ShipToAddress.City,
				Country = request.ShipToAddress.Country
			};
			var deliveryMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(d => d.Id == request.DeliveryMethodId);
			var existingOrder = await _context.Orders
				.Include(o => o.ShippingAddress)
				.Include(o => o.DeliveryMethod)
				.Include(o => o.Items)
				.ThenInclude(oi => oi.Product)
				.FirstOrDefaultAsync(o => o.PaymentIntentId == basket.PaymentIntentId);

			if (existingOrder is not null)
			{
				_context.Orders.Remove(existingOrder);
				await _context.SaveChangesAsync();
				await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
			}

			var order = new Order()
			{
				BuyerEmail = buyerEmail,
				ShippingAddress = address,
				Items = orderitems,
				Subtotal = subtotal,
				DeliveryMethod = deliveryMethod,
				PaymentIntentId = basket.PaymentIntentId!

			};
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
			return Result.Success(order.Adapt<OrderResponse>());
		}
		public async Task<Result<OrderResponse>> GetOrdersForUserAsync(string buyerEmail)
		{
			var orders = await _context.Orders
							.Where(o => o.BuyerEmail == buyerEmail)
							.Include(o => o.ShippingAddress)
							.Include(o => o.Items)
							.ThenInclude(oi => oi.Product)
							.ToListAsync();
			if (orders is null)
				return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);
			var orderResponse = orders.Adapt<OrderResponse>();
			return Result.Success(orderResponse);
		}

		public async Task<Result<OrderResponse>> GetOrderByIdAsync(string buyerEmail,int orderId)
		{
			var order = await _context.Orders
							.Where(o => o.BuyerEmail == buyerEmail && o.Id == orderId)
							.Include(o => o.ShippingAddress)
							.Include(o => o.Items)
							.ThenInclude(oi => oi.Product)
							.FirstOrDefaultAsync();
			if (order is null)
				return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);
			var orderResponse = order.Adapt<OrderResponse>();
			return Result.Success(orderResponse);
		}
		public async Task<Result<IEnumerable<DeliveryMethodResponse>>> GetDeliveryMethodsAsync()
		{
			var deliveryMethods = await _context.DeliveryMethods.ToListAsync();
			if (deliveryMethods is null)
				return Result.Failure<IEnumerable<DeliveryMethodResponse>>(OrderErrors.DeliveryMethodNotFound);

			return Result.Success(deliveryMethods.Adapt<IEnumerable<DeliveryMethodResponse>>());
		}
	}
}
