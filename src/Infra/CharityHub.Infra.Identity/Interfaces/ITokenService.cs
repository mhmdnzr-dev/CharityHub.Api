using System.Security.Claims;

using CharityHub.Core.Domain.Entities.Identity;

namespace CharityHub.Infra.Identity.Interfaces;

using Models;
using Models.Token.Requests;
using Models.Token.Responses;

public interface ITokenService
{
    Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest request);
    Task<GetUserByTokenResponse> GetUserByTokenAsync(GetUserByTokenRequest request);
    ClaimsPrincipal GetUserDetailsFromToken(string token);
}