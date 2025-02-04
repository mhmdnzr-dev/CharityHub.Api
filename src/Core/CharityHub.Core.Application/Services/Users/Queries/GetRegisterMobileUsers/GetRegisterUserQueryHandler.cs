namespace CharityHub.Core.Application.Services.Users.Queries.GetRegisterMobileUsers;

using CharityHub.Core.Application.Primitives;
using CharityHub.Core.Contract.Users.Queries.GetRegisterMobileUsers;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models;

using Microsoft.Extensions.Caching.Memory;

public class GetRegisterUserQueryHandler : QueryHandlerBase<GetRegisterMobileUserQuery, RegisterMobileUserResponseDto>
{
    private readonly IIdentityService _identityService;

    public GetRegisterUserQueryHandler(IMemoryCache cache, ITokenService tokenService, IIdentityService identityService) : base(cache, tokenService)
    {
        _identityService = identityService;
    }
    public override async Task<RegisterMobileUserResponseDto> Handle(GetRegisterMobileUserQuery query, CancellationToken cancellationToken)
    {
        var result = await _identityService.SendOTPAsync(new SendOtpRequest { PhoneNumber = query.PhoneNumber });
        return new RegisterMobileUserResponseDto { IsNewUser = result.IsNewUser,IsSMSSent = result.IsSMSSent };
    }
}