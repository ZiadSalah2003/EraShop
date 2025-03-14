using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Report;
using EraShop.API.Entities;
using EraShop.API.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace EraShop.API.Services
{
    public class ReportService(ApplicationDbContext context , UserManager<ApplicationUser> userManager) : IReportService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<ReportResponse>> GetDailyReport()
        {
            var today = DateTime.UtcNow.Date;

            // Get new users count (excluding admins)
            var allNewUsers = await _context.Users
                                            .Where(x => x.CreatedAt.Date == today)
                                            .ToListAsync();

            var newMemberUsersCount = 0;
            foreach (var user in allNewUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count == 0 || !roles.Contains(DefaultRoles.Admin))
                {
                    newMemberUsersCount++;
                }
            }

            // Get daily orders count and total revenue
            var dailyOrders = await _context.Orders
                                            .Include(o => o.DeliveryMethod)
                                            .Include(o => o.Items)
                                            .ThenInclude(i => i.Product)
                                            .Where(o => o.OrderDate.Date == today)
                                            .ToListAsync();

            var ordersCount = dailyOrders.Count;
            var totalRevenue = dailyOrders.Sum(o => o.GetTotal);

            // Get daily orders details
            var orderDetails = dailyOrders.Select(o => new OrderReportDto(
                o.Id,
                o.BuyerEmail,
                o.Status.ToString(),
                string.Join(" - ", o.ShippingAddress.Street, o.ShippingAddress.City, o.ShippingAddress.Country),
                o.DeliveryMethod?.ShortName ?? "N/A"
            )).ToList();

            // Get sold products
            var soldProducts = dailyOrders.SelectMany(o => o.Items)
                                          .GroupBy(i => i.Product.ProductId)
                                          .Select(g => new SoldProductDto(
                                              g.Key,
                                              g.First().Product.ProductName,
                                              g.First().Product.PictureUrl,
                                              g.Sum(i => i.Quantity)
                                          )).ToList();

            var totalSoldProducts = soldProducts.Count;
            var totalSoldPieces = soldProducts.Sum(p => p.SoldQuantity);

            var reportResponse = new ReportResponse(
                newMemberUsersCount,
                ordersCount,
                totalRevenue,
                totalSoldProducts,
                totalSoldPieces,
                orderDetails,
                soldProducts
            );

            return Result.Success(reportResponse);
        }
    }
}
