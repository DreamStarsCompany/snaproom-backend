using Microsoft.OpenApi.Models;
using SnapRoom.APIs.Middleware;
using SnapRoom.Repositories.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SnapRoom.Contract.Repositories.Mapper;
using SnapRoom.Contract.Repositories.IUOW;
using SnapRoom.Repositories.UOW;
using SnapRoom.Services;
using SnapRoom.Contract.Services;

namespace SnapRoom.APIs
{
	public static class DependencyInjection
	{
		// Main Config method
		public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.ConfigRoute();
			services.ConfigSwagger();
			services.AddDatabase(configuration);
			services.AddAutoMapper();
			services.AddInfrastructure(configuration);
			services.AddServices();
			services.AddCors();
			services.AddHttpContextAccessor();
		}

		public static void ApplicationSetUp(this WebApplication app)
		{
			//app.UseMiddleware<AuthMiddleware>();
			app.UseMiddleware<ExceptionMiddleware>();
		}

		public static void ConfigRoute(this IServiceCollection services)
		{
			services.Configure<RouteOptions>(options =>
			{
				options.LowercaseUrls = true;
			});
		}

		public static void ConfigSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations(); // Enable annotations for Swagger
									   // Add Bearer token support to Swagger
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});
			});

		}

		public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<SnapRoomDbContext>(options =>
			{
				options.UseLazyLoadingProxies() // Enable lazy loading
					   .UseSqlServer(configuration.GetConnectionString("Database")); // Đổi API -> Repositories
				options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
			});
		}

		public static void AddCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigins",
					policy =>
					{
						policy.AllowAnyOrigin()
							  .AllowAnyMethod()
							  .AllowAnyHeader();
					});
			});
		}

		public static void AddAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MapperProfile));
		}

		public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddRepositories();
		}

		public static void AddServices(this IServiceCollection services)
		{
			//services.AddHostedService<SystemBackgroundService>();

			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IConversationService, ConversationService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IPlanService, PlanService>();
			services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<EmailService>();

			services.AddMemoryCache();
		}

		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}
	}
}
