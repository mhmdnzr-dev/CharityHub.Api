using System.Security.Claims;

using CharityHub.Core.Domain.Entities.Identity;

namespace CharityHub.Infra.Identity.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    ClaimsPrincipal GetUserDetailsFromToken(string token);
}
