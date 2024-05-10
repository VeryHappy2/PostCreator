using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Authorization:Authority"];
        var siteAudience = configuration["Authorization:SiteAudience"];

        services
            .AddAuthentication()
            .AddJwtBearer(AuthScheme.Site, options =>
            {
                options.Authority = authority;
                options.Audience = siteAudience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicy.AllowEndUserPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(AuthScheme.Site);
                policy.RequireClaim(JwtRegisteredClaimNames.Sub);
            });
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
}