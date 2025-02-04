namespace CharityHub.Core.Application.Services.Users.Queries.GetUserProfileDetails;

using Contract.Users.Queries.GetUserProfileDetails;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class
    GetUserProfileDetailQueryHandler : QueryHandlerBase<GetUserProfileDetailQuery, UserProfileDetailResponseDto>
{
    public GetUserProfileDetailQueryHandler(IMemoryCache cache, ITokenService tokenService, IHttpContextAccessor httpContextAccessor) : base(cache, tokenService, httpContextAccessor)
    {
    }

    public override async Task<UserProfileDetailResponseDto> Handle(GetUserProfileDetailQuery query, CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader();
        var userDetails=await TokenService.GetUserByTokenAsync(new GetUserByTokenRequest{ Token = token });
        return new UserProfileDetailResponseDto
        {
            UserName = userDetails.UserName,
            PhoneNumber = userDetails.PhoneNumber,
            FirstName = userDetails.FirstName,
            LastName = userDetails.LastName,
        };
    }
}