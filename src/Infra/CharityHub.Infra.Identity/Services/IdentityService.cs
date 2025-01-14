using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CharityHub.Infra.Identity.Services;



public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly string _expireTimeMinutes;
    private readonly IOTPService _otpService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOTPService otpService,
        IOptions<SmsProviderOptions> options
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _expireTimeMinutes = options.Value.ExpireTimeMinutes;
        _otpService = otpService;
    }

    public async Task<bool> SendOTPAsync(string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user is null)
        {
            user = new ApplicationUser { UserName = phoneNumber, PhoneNumber = phoneNumber };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded is not true)
            {
                throw new Exception("Failed to create user.");
            }
        }

        string otp = GenerateOTP();

        user.OTP = otp;
        user.OTPCreationTime = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await _otpService.SendOtpAsync(phoneNumber, otp);
    }

    public async Task<bool> VerifyOTPAsync(string phoneNumber, string otp)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user is null)
        {
            throw new Exception("User not found.");
        }

        if (user.OTP == otp &&
            user.OTPCreationTime.AddMinutes(int.Parse(_expireTimeMinutes)) >= DateTime.UtcNow) // OTP valid for X minutes
        {
            user.OTP = null;
            await _userManager.UpdateAsync(user);

            await AssignRoleToUserAsync(user, "Client");

            return true;
        }

        return false;
    }

    private async Task AssignRoleToUserAsync(ApplicationUser user, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var role = new ApplicationRole(roleName);
            await _roleManager.CreateAsync(role);
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }


    private static string GenerateOTP()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
