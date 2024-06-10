using IdentityServerApi.Host;
using IdentityServerApi.Host.Configurations;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Contracts;
using IdentityServerApi.Host.Repositories.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identityserver.API", Version = "v1" });
});
builder.Services.AddTransient<IUserAccountRepository, UserAccountRepository>();
builder.Services.Configure<IdentityServerApiConfig>(configuration);

builder.Services.AddIdentity<UserApp, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration["ConnectionString"]));

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

app
.UseSwagger()
.UseSwaggerUI(setup =>
{
    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Identityserver.API V1");
    setup.OAuthAppName("Identityserver Swagger UI");
});

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

await AddRolesAsync(app);
await AddAdminAsync(app);

app.Use(async (context, next) =>
{
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

async Task AddAdminAsync(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var userManager = services.GetRequiredService<UserManager<UserApp>>();

            var adminUserExists = await userManager.FindByEmailAsync("admin@super.com");

            if (adminUserExists == null)
            {
                var newAdmin = new UserApp()
                {
                    UserName = "admin",
                    Email = "admin@super1.com",
                };

                var result = await userManager.CreateAsync(newAdmin, "s@dE12upe");

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

            string[] roles = { AuthRoles.Admin, AuthRoles.User };

            foreach (var role in roles)
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
