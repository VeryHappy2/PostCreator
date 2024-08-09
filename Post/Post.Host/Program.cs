using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Post.Host.Configurations;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services;
using Post.Host.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.Filters.Add(typeof(HttpGlobalExceptionFilter));
})

.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Post HTTP API",
		Version = "v1",
		Description = "The Post Service HTTP API",
    });

    var authority = configuration["Authorization:Authority"];

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer' followed by a space and the JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication()
  .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddControllers();
builder.Services.Configure<PostConfig>(configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IPostBffRepository,  PostBffRepository>();
builder.Services.AddTransient<IPostBffService, PostBffService>();
builder.Services.AddTransient<IService<PostItemEntity>, PostItemService>();
builder.Services.AddTransient<IService<PostCommentEntity>, PostCommentService>();
builder.Services.AddTransient<IService<PostCategoryEntity>, PostCategoryService>();

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

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
});

app.Use(async (context, next) =>
{
    if (context.Request.Cookies.ContainsKey("token") &&
       !context.Request.Headers.ContainsKey("Authorization"))
    {
        var token = context.Request.Cookies["token"];
        context.Request.Headers.Add("Authorization", $"Bearer {token}");
    }

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var id = Guid.NewGuid();
    LogRequest(logger, context.Request, id);

    await next.Invoke();

    LogResponse(logger, context.Response, id);
});

app
.UseSwagger()
.UseSwaggerUI(setup =>
{
	setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Post.API V1");
	setup.OAuthClientId("postswaggerui");
	setup.OAuthAppName("Post Swagger UI");
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

			context.Database.EnsureCreatedAsync().Wait();
			DbInitializer.Initialize(context).Wait();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred creating the DB.");
		}
	}
}

app.Run();