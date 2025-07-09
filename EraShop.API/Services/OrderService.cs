using EraShop.API.Abstractions;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Contracts.Orders;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Order;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace EraShop.API.Services
{
	public class OrderService : IOrderService
	{
		private readonly IBasketService _basketService;
		private readonly IProductService _productService;
		private readonly IPaymentService _paymentService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		public OrderService(IProductService productService, IPaymentService paymentService, IBasketService basketService, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
		{
			_basketService = basketService;
			_productService = productService;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
			_userManager = userManager;
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
			
			var deliveryMethodRepository = _unitOfWork.GetRepository<DeliveryMethod, int>();
			var deliveryMethod = await deliveryMethodRepository.GetByIdAsync(request.DeliveryMethodId);
			
			var orderRepository = _unitOfWork.GetRepository<Order, int>();
			var existingOrderSpec = new OrderSpecification(o => o.PaymentIntentId == basket.PaymentIntentId);
			var existingOrder = await orderRepository.GetWithSpecAsync(existingOrderSpec);

			if (existingOrder is not null)
			{
				orderRepository.Delete(existingOrder);
				await _unitOfWork.CompleteAsync();
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

			var user = await _userManager.FindByEmailAsync(buyerEmail);
			if (user is null)
				return Result.Failure<OrderResponse>(UserErrors.UserEmailNotFound);

			var billRepository = _unitOfWork.GetRepository<Bill, int>();
			var bill = new Bill
			{
				BuyerName = $"{user.FirstName} {user.LastName}",
				BuyerEmail = buyerEmail,
				Subtotal = subtotal,
				Status = OrderStatus.Pending
			};
			await billRepository.AddAsync(bill);
			await orderRepository.AddAsync(order);
			await _unitOfWork.CompleteAsync();
			return Result.Success(order.Adapt<OrderResponse>());
		}
		public async Task<Result<IEnumerable<OrderResponse>>> GetOrdersForUserAsync(string buyerEmail)
		{
			var orderRepository = _unitOfWork.GetRepository<Order, int>();
			var orderSpec = new OrderSpecification(buyerEmail);
			var orders = await orderRepository.GetAllWithSpecAsync(orderSpec);
			
			if (orders == null || !orders.Any())
				return Result.Failure< IEnumerable<OrderResponse>> (OrderErrors.OrderNotFound);
			var orderResponse = orders.Adapt<IEnumerable<OrderResponse>>();
			return Result.Success(orderResponse);
		}

		public async Task<Result<OrderResponse>> GetOrderByIdAsync(string buyerEmail,int orderId)
		{
			var orderRepository = _unitOfWork.GetRepository<Order, int>();
			var orderSpec = new OrderSpecification(o => o.BuyerEmail == buyerEmail && o.Id == orderId);
			var order = await orderRepository.GetWithSpecAsync(orderSpec);
			
			if (order is null)
				return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);
			var orderResponse = order.Adapt<OrderResponse>();
			return Result.Success(orderResponse);
		}
		public async Task<Result<IEnumerable<DeliveryMethodResponse>>> GetDeliveryMethodsAsync()
		{
			var deliveryMethodRepository = _unitOfWork.GetRepository<DeliveryMethod, int>();
			var deliveryMethods = await deliveryMethodRepository.GetAllAsync();
			
			if (deliveryMethods == null || !deliveryMethods.Any())
				return Result.Failure<IEnumerable<DeliveryMethodResponse>>(OrderErrors.DeliveryMethodNotFound);

			return Result.Success(deliveryMethods.Adapt<IEnumerable<DeliveryMethodResponse>>());
		}
	}
}
