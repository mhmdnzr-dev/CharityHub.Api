using System.Security.Claims;
using System.Text;

using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Services;
using CharityHub.Infra.Sql.Data.DbContexts;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace CharityHub.Infra.Identity;



public static class DependencyInjection
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<CharityHubCommandDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureAuthentication(configuration);
        services.ConfigureAuthorization();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOTPService, KavenegarOtpService>();
    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("Identity", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (context.Principal is not null)
                    {
                        var userClaims = context.Principal.Claims.ToList();
                        var roles = userClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                        var customClaim = userClaims.FirstOrDefault(c => c.Type == "CustomClaimType")!.Value;
                    }
                    return Task.CompletedTask;
                }
            };
        })
        .AddJwtBearer("OpenId", options =>
        {
            options.Authority = configuration["OpenId:Authority"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        });
    }

    private static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "scope_sapplus");
            });

            options.AddPolicy("CanViewDonations", policy =>
                policy.RequireRole("Admin", "Manager"));
        });
    }
} 
