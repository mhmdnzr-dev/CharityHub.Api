using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;


namespace CharityHub.Infra.Identity.Services;

public class SignupService
{
    private readonly IIdentityService _identityService;

    public SignupService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> SignUpAsync(string email, string password, string role = null)
    {
        var user = new ApplicationUser { Email = email, UserName = email };
        var result = await _identityService.CreateUserAsync(user, password);

        if (!result.Succeeded)
        {
            return false; // User creation failed
        }

        if (!string.IsNullOrEmpty(role))
        {
            var roleResult = await _identityService.AddUserToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                await _identityService.DeleteUserAsync(user); // Rollback user creation
                return false;
            }
        }

        return true;
    }
}