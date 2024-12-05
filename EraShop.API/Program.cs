
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
			app.UseStaticFiles( new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(builder.Environment.ContentRootPath,
					"Uploads")),
				RequestPath ="/Resources"
			});
			app.UseCors();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
