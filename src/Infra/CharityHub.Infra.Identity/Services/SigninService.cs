namespace CharityHub.Infra.Identity.Services;
using CharityHub.Infra.Identity.Interfaces;

public class SigninService
{
    private readonly IIdentityService _identityService;

    public SigninService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        var user = await _identityService.FindUserByEmailAsync(email);
        if (user == null)
        {
            return false; // User not found
        }

        // Assuming you have a method to verify the password (e.g., SignInManager or a custom implementation)
        var isPasswordValid = await _identityService.CheckPasswordAsync(user, password);
        return isPasswordValid;
    }
}