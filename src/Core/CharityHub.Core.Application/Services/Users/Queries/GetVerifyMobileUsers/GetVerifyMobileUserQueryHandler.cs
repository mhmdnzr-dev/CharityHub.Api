namespace CharityHub.Core.Application.Services.Users.Queries.GetVerifyMobileUsers;

using Contract.Users.Queries.GetVerifyMobileUsers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetVerifyMobileUserQueryHandler : QueryHandlerBase<GetVerifyMobileUserQuery, VerifyMobileUserResponseDto>
{
    private readonly IIdentityService _identityService;

    public GetVerifyMobileUserQueryHandler(IMemoryCache cache, ITokenService tokenService, IIdentityService identityService) : base(cache, tokenService)
    {
        _identityService = identityService;
    }

    public override async Task<VerifyMobileUserResponseDto> Handle(GetVerifyMobileUserQuery query,
        CancellationToken cancellationToken)
    {
        var result =
            await _identityService.VerifyOTPAndGenerateTokenAsync(new VerifyOtpRequest
            {
                PhoneNumber = query.PhoneNumber, OtpCode = query.OTPCode
            });
        return new VerifyMobileUserResponseDto
        {
            PhoneNumber = query.PhoneNumber, Name = result.Name, Token = result.Token
        };
    }
}