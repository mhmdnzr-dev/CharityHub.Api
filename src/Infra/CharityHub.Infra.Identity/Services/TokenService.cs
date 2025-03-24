using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CharityHub.Infra.Identity.Services;

using System.Security.Cryptography;

using Core.Contract.Configuration.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Models;
using Models.Token.Requests;
using Models.Token.Responses;

using Sql.Data.DbContexts;

public class TokenService : ITokenService
{
    private readonly IOptions<JwtOptions> _options;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CharityHubCommandDbContext _commandDbContext;

    private readonly IHttpContextAccessor _httpContextAccessor;
    public TokenService(IOptions<JwtOptions> options, UserManager<ApplicationUser> userManager,
        CharityHubCommandDbContext commandDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _options = options;
        _userManager = userManager;
        _commandDbContext = commandDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest request)
    {
        if (request.User == null)
            throw new ArgumentNullException(nameof(request.User), "User cannot be null.");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, request.User.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, request.User.UserName)
        };

        var roles = await _userManager.GetRolesAsync(request.User);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _options.Value.Issuer,
            audience: _options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        var userToken = await _commandDbContext.ApplicationUserTokens
            .FirstOrDefaultAsync(t => t.UserId == request.User.Id);

        if (userToken == null)
        {
            userToken = new ApplicationUserToken
            {
                UserId = request.User.Id,
                LoginProvider = "JWT",
                Name = "AccessToken",
                Value = accessTokenString,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            _commandDbContext.ApplicationUserTokens.Add(userToken);
        }
        else
        {
            userToken.Value = accessTokenString;
            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpiration = refreshTokenExpiration;
        }

        await _commandDbContext.SaveChangesAsync();

        return new GenerateTokenResponse { Token = accessTokenString, RefreshToken = refreshToken };
    }

    public async Task<GenerateTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _commandDbContext.ApplicationUserTokens
            .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken && t.RefreshTokenExpiration > DateTime.UtcNow);

        if (storedToken == null)
            throw new SecurityTokenException("Invalid or expired refresh token.");

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user == null)
            throw new SecurityTokenException("User not found.");

        var newTokens = await GenerateTokenAsync(new GenerateTokenRequest { User = user });

        storedToken.Value = newTokens.Token;
        storedToken.RefreshToken = newTokens.RefreshToken;
        storedToken.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        await _commandDbContext.SaveChangesAsync();

        return newTokens;
    }

    public async Task<GetUserByTokenResponse> GetUserByTokenAsync(GetUserByTokenRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new SecurityTokenException("User ID claim not found in token.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new SecurityTokenException("User not found.");

        return new GetUserByTokenResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
         
        };
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        return await _commandDbContext.ApplicationUserTokens.AnyAsync(t => t.Value == token);
    }

    private ClaimsPrincipal GetUserDetailsFromToken(string token)
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