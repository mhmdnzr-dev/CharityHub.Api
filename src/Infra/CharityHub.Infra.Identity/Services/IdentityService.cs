using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Utils.Extensions.Extensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CharityHub.Infra.Identity.Services;



public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly string _expireTimeMinutes;
    private readonly IOTPService _otpService;
    private readonly ITokenService _tokenService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOTPService otpService,
        IOptions<SmsProviderOptions> options,
        ITokenService tokenService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _expireTimeMinutes = options.Value.ExpireMinute;
        _otpService = otpService;
        _tokenService = tokenService;
    }

    public async Task<bool> SendOTPAsync(string phoneNumber, bool acceptedTerms)
    {
        if (!PhoneNumberHelpers.IsValidIranianMobileNumber(phoneNumber))
        {
            throw new Exception("Mobile number isn't valid!");
        }

        if (!acceptedTerms)
        {
            throw new Exception("You have to accept terms then try to login!");
        }
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

        // TODO: replace await _otpService.SendOTPAsync(phoneNumber, otp) instead of true in production
        var isSMSSent = true;

        return isSMSSent;
    }


    public async Task<string> VerifyOTPAndGenerateTokenAsync(string phoneNumber, string otp)
    {
        // Find user by phone number
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user is null)
        {
            throw new Exception("User not found.");
        }

        // Check if OTP is expired
        var isTokenExpired = user.OTPCreationTime.AddMinutes(int.Parse(_expireTimeMinutes)) < DateTime.UtcNow;

        // TODO: replace user.OTP in production
        var isOTPValid = "522368" == otp;

        // Validate OTP
        if (isOTPValid && !isTokenExpired) // OTP valid for X minutes
        {
            // Assign role to user if OTP is valid
            await AssignRoleToUserAsync(user, "Client");

            // Generate and return token
            var token = await _tokenService.GenerateTokenAsync(user);
            return token;
        }

        throw new Exception("Invalid or expired OTP.");
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
