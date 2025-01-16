using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CharityHub.Infra.Identity.Services;



public class TokenService : ITokenService
{
    private readonly IOptions<JwtOptions> _options;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IOptions<JwtOptions> options, UserManager<ApplicationUser> userManager)
    {
        _options = options;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber), // Replace email with phone number
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        // Add user roles as claims
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Generate signing key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the JWT
        var token = new JwtSecurityToken(
            issuer: _options.Value.Issuer,
            audience: _options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal GetUserDetailsFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.Value.Key);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _options.Value.Issuer,
            ValidAudience = _options.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        return principal;
    }
}