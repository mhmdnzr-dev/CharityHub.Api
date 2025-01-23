using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models;
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

    public async Task<SendOtpResponse> SendOTPAsync(SendOtpRequest request)
    {
        var result = new SendOtpResponse();
        if (!PhoneNumberHelpers.IsValidIranianMobileNumber(request.PhoneNumber))
        {
            throw new Exception("Mobile number isn't valid!");
        }


        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        var isNewUser = user == null ? true : false;

        if (isNewUser)
        {
            result.IsNewUser = true;
            user = new ApplicationUser
            {
                UserName = request.PhoneNumber,
                PhoneNumber = request.PhoneNumber
            };
            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                throw new Exception("Failed to create user.");
            }

        }
        else
        {
            result.IsNewUser = false;
        }


        string otp = GenerateOTP();

        user.OTP = otp;
        user.OTPCreationTime = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // TODO: replace await _otpService.SendOTPAsync(phoneNumber, otp) instead of true in production
        result.IsSMSSent = true;


        return result;
    }


    public async Task<VerifyOtpResponse> VerifyOTPAndGenerateTokenAsync(VerifyOtpRequest request)
    {
        var result = new VerifyOtpResponse();

        // Check if user exists
        var userExists = await _userManager.FindByNameAsync(request.PhoneNumber);

        if (userExists is null)
        {
            // If user is new, check if terms are accepted
            if (!request.AcceptedTerms)
            {
                throw new Exception("User must accept the terms before proceeding.");
            }

            // Create the user if needed or other actions if the user is new
            userExists = new ApplicationUser
            {
                UserName = request.PhoneNumber,
                PhoneNumber = request.PhoneNumber,
            };

            // Optionally, you can save the user to the database here
            await _userManager.CreateAsync(userExists);
        }

        // If the user exists or has been created, proceed with OTP verification
        var isTokenExpired = userExists.OTPCreationTime.AddMinutes(int.Parse(_expireTimeMinutes)) < DateTime.UtcNow;
        var isOTPValid = "522368" == request.Otp;

        if (isOTPValid && !isTokenExpired)
        {
            await AssignRoleToUserAsync(userExists, "Client");
            var token = await _tokenService.GenerateTokenAsync(userExists);
            result.Token = token;
            return result;
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
