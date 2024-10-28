using IdentityServerApi.Host.Configurations;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Repositories;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Identity;
using Infrastructure.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.RateLimiting;

var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Identity HTTP API",
        Version = "v1",
        Description = "The Identity Service HTTP API",
    });

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

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(configuration["RateLimiter:Policies:0:Name"], options =>
    {
        options.PermitLimit = int.Parse(configuration["RateLimiter:Policies:0:PermitLimit"]);
        options.Window = TimeSpan.FromSeconds(10);
    });

    options.AddFixedWindowLimiter(configuration["RateLimiter:Policies:1:Name"], options =>
    {
        options.PermitLimit = int.Parse(configuration["RateLimiter:Policies:1:PermitLimit"]);
        options.Window = TimeSpan.FromSeconds(300);
    });
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddTransient<IUserManagmentService, UserManagmentService>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();
builder.Services.AddTransient<IUserBffAccountService, UserBffAccountService>();
builder.Services.AddTransient<IUserBffAccountRepository, UserBffAccountRepository>();
builder.Services.AddTransient<IUserAuthenticationRepository, UserAuthenticationRepository>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.Configure<IdentityServerApiConfig>(configuration);

builder.Services.AddIdentity<UserApp, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

builder.Services.AddCors(options =>
{
	options.AddPolicy(
		"CorsPolicy",
		builder => builder
            .WithOrigins("http://www.postcreator.com", "http://localhost:4200") // http://localhost:4200 - for testing front-end
            .AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials());
});

builder.Services.AddAuthorization(configuration);

var app = builder.Build();

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

CreateDbIfNotExists(app);
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
});

app
.UseSwagger()
.UseSwaggerUI(setup =>
{
    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Identityserver.API V1");
    setup.OAuthAppName("Identityserver Swagger UI");
});

app.UseRateLimiter();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

await AddRolesAsync(app);

await AddDefaultAdminAsync(app);

void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}

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

async Task AddDefaultAdminAsync(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        string emailAdmin = "admin124qwc5@gmail.com";
        string passwordAdmin = "#dc!9cjWkaqEl(&2m";

        try
        {
            var userManager = services.GetRequiredService<UserManager<UserApp>>();

            var adminUserExists = await userManager.FindByEmailAsync(emailAdmin);

            if (adminUserExists == null)
            {
                var newAdmin = new UserApp()
                {
                    UserName = "admin",
                    Email = emailAdmin,
                };

                var result = await userManager.CreateAsync(newAdmin, passwordAdmin);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
                    logger.LogError("An error occured while adding admin");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while admin.");
        }
    }
}

async Task AddRolesAsync(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in AuthRoles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while adding roles.");
        }
    }
}

app.Run();
