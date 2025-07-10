using EraShop.API.Authentication;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Entities;
using EraShop.API.Persistence;
using EraShop.API.Persistence.UnitOfWork;
using EraShop.API.Services;
using EraShop.API.Settings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Hangfire;
using FluentValidation.AspNetCore;
using FluentValidation;
using EraShop.API.Helpers;
namespace EraShop.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCors(options =>
				options.AddDefaultPolicy(builder =>
				builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowAnyOrigin()
				)
			);

			services.AddControllers();

			var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String DefaultConnection not found.");
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailSender, EmailService>();
      			services.AddScoped<IFileService, CloudinaryService>();
            services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SectionName));
            services.AddScoped<IBrandService,BrandService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IBasketService, BasketService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IReviewService, ReviewService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<INotificationService, NotificationService>();
			services.AddScoped<IWishListService, WishListService>();




			services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
			services.AddSwaggerServices();


			services.AddAuthConfig(configuration);
			services.ReddisConfiguration(configuration);
			services.AddMapsterConfig();
			services.AddFluentValidationConfig();
			

			services.AddHttpContextAccessor();
			services.AddBackgroundJobsConfig(configuration);
			// mapping MailSettings

			services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

			return services;
		}
		private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			
			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Enter the Bearer Authroization : `Bearer Generated-JWT-Token`",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme
						}
					}, new string []{}
					}
				});
			});
			

			return services;
		}

        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }

		private static IServiceCollection ReddisConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton(typeof(IConnectionMultiplexer), servicesProvider =>
			{
				var connectionString = configuration.GetConnectionString("Redis");
				var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);
				return connectionMultiplexerObj;
			});
			return services;
		}

        private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            services.AddHangfireServer();
            return services;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<IJwtProvider, JwtProvider>();
			services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();

			var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(o =>
				{
					o.SaveToken = true;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
						ValidIssuer = jwtSettings?.Issuer,
						ValidAudience = jwtSettings?.Audience
					};
				});
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequiredLength = 8;
				options.SignIn.RequireConfirmedEmail = true;
				options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                       "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

			return services;
		}
	}
}
