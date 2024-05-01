using Infrastructure.Extensions;
using Infrastructure.Filters;
using Microsoft.OpenApi.Models;
using Order.Host.Configurations;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Repositories;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services;
using Order.Host.Services.Interfaces;
using Post.Host.Configurations;

var configuration = GetConfiguration();

//build config

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.Filters.Add(typeof(HttpGlobalExceptionFilter));
})
	.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "eShop- Order HTTP API",
		Version = "v1",
		Description = "The Order Service HTTP API"
	});

	var authority = configuration["Authorization:Authority"];
	options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.OAuth2,
		Flows = new OpenApiOAuthFlows()
		{
			Implicit = new OpenApiOAuthFlow()
			{
				AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
				TokenUrl = new Uri($"{authority}/connect/token"),
				Scopes = new Dictionary<string, string>()
				{
					{ "mvc", "website" },
					{ "order.item", "order.item" },
					{ "order.order", "order.order" },
				}
			}
		}
	});

	options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddControllers();
builder.Services.Configure<PostConfig>(configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IOrderBffRepository,  PostBffRepository>();
builder.Services.AddTransient<IRepository<OrderItemEntity>, OrderItemRepository>();
builder.Services.AddTransient<IOrderBffService, PostBffService>();
builder.Services.AddTransient<IService<OrderItemEntity>, PostItemService>();
builder.Services.AddTransient<IService<OrderEntity>, PostCommentService>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

builder.Services.AddCors(options =>
{
	options.AddPolicy(
		"CorsPolicy",
		builder => builder
			.SetIsOriginAllowed((host) => true)
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials());
});
builder.Services.AddAuthorization(configuration);

//app option

var app = builder.Build();

app.UseSwagger()
	.UseSwaggerUI(setup =>
	{
		setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Order.API V1");
		setup.OAuthClientId("orderswaggerui");
		setup.OAuthAppName("Order Swagger UI");
	});

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
	var id = Guid.NewGuid();
    LogRequest(logger, context.Request, id);

    // Call the next middleware in the pipeline
    await next.Invoke();

    LogResponse(logger, context.Response, id);
});

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

CreateDbIfNotExists(app);

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void LogRequest(ILogger<Program> logger, HttpRequest request, Guid id)
{
	logger.LogInformation($"Request id:{id}, Method: {request.Method}, Path {request.Path}");
}

void LogResponse(ILogger<Program> logger, HttpResponse response, Guid id)
{
    logger.LogInformation($"Response id: {id}, Status: {response.StatusCode}");
}

void CreateDbIfNotExists(IHost host)
{
	using (var scope = host.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		var logger = services.GetRequiredService<ILogger<Program>>();
		try
		{
			var context = services.GetRequiredService<ApplicationDbContext>();

			var check = context.Database.EnsureCreated();
			logger.LogInformation($"Status: {check}");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred creating the DB.");
		}
	}
}