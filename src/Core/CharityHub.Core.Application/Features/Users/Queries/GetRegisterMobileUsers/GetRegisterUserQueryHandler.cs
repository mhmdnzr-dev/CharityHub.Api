namespace CharityHub.Core.Application.Features.Users.Queries.GetRegisterMobileUsers;

using Contract.Features.Users.Queries.GetRegisterMobileUsers;

using Primitives;
using Infra.Identity.Interfaces;

using Infra.Identity.Models.Identity.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

public class GetRegisterUserQueryHandler : QueryHandlerBase<GetRegisterMobileUserQuery, RegisterMobileUserResponseDto>
{
    private readonly IIdentityService _identityService;

    public GetRegisterUserQueryHandler(IMemoryCache cache, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IIdentityService identityService) : base(cache, tokenService, httpContextAccessor)
    {
        _identityService = identityService;
    }
    
    
    public override async Task<RegisterMobileUserResponseDto> Handle(GetRegisterMobileUserQuery query, CancellationToken cancellationToken)
    {
        var result = await _identityService.SendOTPAsync(new SendOtpRequest { PhoneNumber = query.PhoneNumber });
        return new RegisterMobileUserResponseDto { IsNewUser = result.IsNewUser,IsSMSSent = result.IsSMSSent };
    }
}