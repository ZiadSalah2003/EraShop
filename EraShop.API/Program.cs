
using EraShop.API.Services;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace EraShop.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, configuration) =>
             configuration.ReadFrom.Configuration(context.Configuration)
              );
            builder.Services.AddDependencies(builder.Configuration);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization =
                    [
                        new HangfireCustomBasicAuthenticationFilter
                        {
                            User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
                            Pass =app.Configuration.GetValue<string>("HangfireSettings:Password")
                        }
                    ],
                DashboardTitle = "EraShop Dashboard"
            });


            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
			using var scope = scopeFactory.CreateScope();
			var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

			RecurringJob.AddOrUpdate("SendWeeklyUserEmails",() => notificationService.SendForUsersNotifications(),"0 12 * * 6");
			app.UseStaticFiles();
			app.UseCors();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
