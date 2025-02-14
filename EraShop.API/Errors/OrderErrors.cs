using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class OrderErrors
	{
		public static readonly Error OrderNotFound = new("Orders.NotFound", "No Orders was found with the given id", StatusCodes.Status404NotFound);
		public static readonly Error BasketRetriveError = new("Basket.RetriveFailed", "Failed to Retrive the Basket", StatusCodes.Status400BadRequest);
		public static readonly Error DeliveryMethodNotFound = new("Delivery.NotFound", "No Delivery was found", StatusCodes.Status404NotFound);
		public static readonly Error PaymentIntentNotFound = new("Payment.NotFound", "No Payment Intent was found", StatusCodes.Status404NotFound);

	}
}
