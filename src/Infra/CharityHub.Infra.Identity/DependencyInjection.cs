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
    public static void AddIdentity(this IServiceCollection services,IConfiguration configuration)
    {
 

        // Register Identity services
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<CharityHubCommandDbContext>()
            .AddDefaultTokenProviders();


        // Register policies for access control
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanViewDonations", policy =>
                policy.RequireRole("Admin", "Manager"));
        });

        // Add authentication and authorization
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
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
                    var userClaims = context.Principal.Claims;
                    // Example: Extract roles or custom claims
                    var roles = userClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                    var customClaim = userClaims.FirstOrDefault(c => c.Type == "CustomClaimType")?.Value;

                    // Perform additional validation or processing if necessary
                    return Task.CompletedTask;
                }
            };
        });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOTPService, KavenegarOtpService>();

    }
}
