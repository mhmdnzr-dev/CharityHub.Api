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

using Models;
using Models.Token.Requests;
using Models.Token.Responses;

public class TokenService : ITokenService
{
    private readonly IOptions<JwtOptions> _options;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IOptions<JwtOptions> options, UserManager<ApplicationUser> userManager)
    {
        _options = options;
        _userManager = userManager;
    }

    public async Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest request)
    {
        if (request.User == null)
            throw new ArgumentNullException(nameof(request.User), "User cannot be null.");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.User.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, request.User.PhoneNumber), // Replace email with phone number
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, request.User.UserName)
        };

        // Add user roles as claims
        var roles = await _userManager.GetRolesAsync(request.User);
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
        var result = new GenerateTokenResponse { Token = new JwtSecurityTokenHandler().WriteToken(token), };
        return result;
    }

    public async Task<GetUserByTokenResponse> GetUserByTokenAsync(GetUserByTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
            throw new ArgumentNullException(nameof(request.Token), "Token cannot be null or empty.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.Value.Key);

        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.Value.Issuer,
                ValidAudience = _options.Value.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // Prevents allowing expired tokens due to default clock skew
            };

            var principal = tokenHandler.ValidateToken(request.Token, parameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwtToken)
                throw new SecurityTokenException("Invalid token");

            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new SecurityTokenException("User ID claim not found in token.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new SecurityTokenException("User not found.");

            // Retrieve user roles
            var roles = await _userManager.GetRolesAsync(user);

            return new GetUserByTokenResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FristName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Token validation failed.", ex);
        }
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