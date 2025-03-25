namespace CharityHub.Core.Application.Features.Users.Commands.UpdateUserProfiles;

using Contract.Features.Users.Commands.UpdateUserProfiles;

using Domain.Entities.Identity;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Primitives;

public class UpdateUserProfileCommandHandler : CommandHandlerBase<UpdateUserProfileCommand>
{
 
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ILogger<UpdateUserProfileCommandHandler> logger) : base(tokenService, httpContextAccessor)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public override async Task<int> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var userDetails = await GetUserDetailsAsync();

        var existingUser = await _userManager.FindByIdAsync(userDetails.Id.ToString());
        if (existingUser == null)
        {
            _logger.LogError("User not found");
            throw new Exception("User not found");
        }

        existingUser.FirstName = command.FirstName;
        existingUser.LastName = command.LastName;

        var result = await _userManager.UpdateAsync(existingUser);

        if (!result.Succeeded)
        {
            _logger.LogError("Error updating user profile: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
            throw new Exception($"Error updating user profile: {string.Join(", ", result.Errors)}");
        }

        // Step 6: Optionally log the update success
        _logger.LogInformation("User profile updated successfully for user {UserId}", existingUser.Id);


        return userDetails.Id;
    }
}