using EraShop.API.Entities;

namespace EraShop.API.Services
{
    public interface INotificationService
    {
        Task SendNewProductsNotifications(Product product);
    }
}
