namespace CharityHub.Core.Application.Services.Users.Commands.UpdateUserProfiles;

using Contract.Users.Commands.UpdateUserProfiles;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Identity.Requests;

using Primitives;

public class UpdateUserProfileCommandHandler : CommandHandlerBase<UpdateUserProfileCommand>
{
    private readonly IIdentityService _identityService;

    public UpdateUserProfileCommandHandler(ITokenService tokenService, IIdentityService identityService) :
        base(tokenService)
    {
        _identityService = identityService;
    }

    public override async Task<int> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader();
        var result = await _identityService.UpdateProfileAsync(new UpdateProfileRequest
        {
            Token = token, FirstName = request.FirstName, LastName = request.LastName
        });
        return result.Id;
    }
}