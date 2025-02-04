namespace CharityHub.Core.Application.Services.Users.Queries.GetLogoutMobileUsers;

using Contract.Users.Queries.GetLogoutMobileUsers;
using Contract.Users.Queries.GetRegisterMobileUsers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetLogoutMobileUserQueryHandler : QueryHandlerBase<GetLogoutMobileUserQuery, LogoutMobileUserResponseDto>
{
    private readonly IIdentityService _identityService;

    public GetLogoutMobileUserQueryHandler(IMemoryCache cache, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IIdentityService identityService) : base(cache, tokenService, httpContextAccessor)
    {
        _identityService = identityService;
    }
    public override async Task<LogoutMobileUserResponseDto> Handle(GetLogoutMobileUserQuery query,
        CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader();
        var result = await _identityService.LogoutAsync(new LogoutRequest { Token = token });
        return new LogoutMobileUserResponseDto { IsLogout = result };
    }
}