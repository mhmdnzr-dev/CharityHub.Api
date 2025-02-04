namespace CharityHub.Core.Application.Services.Users.Queries.GetLogoutMobileUsers;

using Contract.Users.Queries.GetLogoutMobileUsers;
using Contract.Users.Queries.GetRegisterMobileUsers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetLogoutMobileUserQueryHandler : QueryHandlerBase<GetLogoutMobileUserQuery, LogoutMobileUserResponseDto>
{
    private readonly IIdentityService _identityService;

    public GetLogoutMobileUserQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IIdentityService identityService) : base(cache, tokenService)
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