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

var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
});
//.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
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

builder.Services.AddTransient<IUserAccountService, UserAccountService>();
builder.Services.AddTransient<IUserBffAccountService, UserBffAccountService>();
builder.Services.AddTransient<IUserBffAccountRepository, UserBffAccountRepository>();
builder.Services.Configure<IdentityServerApiConfig>(configuration);
builder.Services.AddSwaggerGen();

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
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddAuthorization(configuration);

var app = builder.Build();
CreateDbIfNotExists(app);
app.UseCookiePolicy();

app
.UseSwagger()
.UseSwaggerUI(setup =>
{
    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Identityserver.API V1");
    setup.OAuthAppName("Identityserver Swagger UI");
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

await AddRolesAsync(app);

await AddDefaultAdminAsync(app);

app.Use(async (context, next) =>
{
    // if (context.Request.Cookies.ContainsKey("jwt") &&
    //     !context.Request.Headers.ContainsKey("Authorization"))
    // {
    //     var token = context.Request.Cookies["jwt"];
    //     context.Request.Headers.Add("Authorization", $"Bearer {token}");
    // }

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var id = Guid.NewGuid();
    LogRequest(logger, context.Request, id);

    await next.Invoke();

    LogResponse(logger, context.Response, id);
});

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
        try
        {
            var userManager = services.GetRequiredService<UserManager<UserApp>>();

            var adminUserExists = await userManager.FindByEmailAsync("admin124qwc5@gmail.com");

            if (adminUserExists == null)
            {
                var newAdmin = new UserApp()
                {
                    UserName = "admin",
                    Email = "admin124qwc5@gmail.com",
                };

                var result = await userManager.CreateAsync(newAdmin, "#dc!9cjWkaqEl(&2m");

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
