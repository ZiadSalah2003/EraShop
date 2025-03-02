using EraShop.API.Entities;
using EraShop.API.Helpers;
using EraShop.API.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EraShop.API.Services
{
    public class NotificationService(ApplicationDbContext context
        , IHttpContextAccessor httpContextAccessor
        , IEmailSender emailSender
        , UserManager<ApplicationUser> userManager) : INotificationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task SendNewProductsNotifications(Product product)
        {
            var users = await _userManager.Users.Where(x => x.EmailConfirmed && !x.IsDisabled).ToListAsync();
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            foreach (var user in users)
            {
                var placeholders = new Dictionary<string, string>
                    {
                        {"{{UserName}}", user.FirstName },
                        {"{{ProductName}}", product.Name },
                        {"{{ProductDescription}}", product.Description},
                        {"{{ProductPrice}}", product.Price.ToString()},
                        {"{{ProductLink}}",$"{origin}/Product/{product.Id}" }


                    };

                var body = EmailBodyBuilder.GenerateEmailBody("ProductNotification", placeholders);


                BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, $"📣 EraShop : New Product {product.Name}", body));
            }
        }
		public async Task SendForUsersNotifications()
		{
			var usersRule = await _userManager.GetUsersInRoleAsync("Member");

			var users = usersRule.Where(x => x.EmailConfirmed && !x.IsDisabled).ToList();

			foreach (var user in users)
			{
				var placeholders = new Dictionary<string, string>
		        {
			        {"{{UserName}}", user.FirstName }
		        };
				var body = EmailBodyBuilder.GenerateEmailBody("UsersNotification", placeholders);
				BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "📢 Weekly Update: Visit EraShop Now!", body));
			}
		}
	}
}
