using System.Security.Claims;

using CharityHub.Core.Domain.Entities.Identity;

namespace CharityHub.Infra.Identity.Interfaces;

using Models;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    Task<UserWithRolesDtoModel> GetUserByTokenAsync(string token);
    ClaimsPrincipal GetUserDetailsFromToken(string token);
}