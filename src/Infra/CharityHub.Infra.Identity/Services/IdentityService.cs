using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CharityHub.Infra.Identity.Services;

using System.Security.Claims;
using System.Text;

using CharityHub.Core.Domain.Enums;

using Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sql.Data.DbContexts;

using Utils.Extensions.Extensions;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly string _expireTimeMinutes;
    private readonly IOTPService _otpService;
    private readonly ITokenService _tokenService;
    private readonly CharityHubQueryDbContext _queryDbContext;
    private readonly CharityHubCommandDbContext _commandDbContext;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOTPService otpService,
        IOptions<SmsProviderOptions> options,
        ITokenService tokenService,
        CharityHubQueryDbContext queryDbContext,
        CharityHubCommandDbContext commandDbContext,
        ILogger<IdentityService> logger
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _expireTimeMinutes = options.Value.ExpireMinute;
        _otpService = otpService;
        _tokenService = tokenService;
        _queryDbContext = queryDbContext;
        _commandDbContext = commandDbContext;
        _logger = logger;
    }

    public async Task<SendOtpResponse> SendOTPAsync(SendOtpRequest request)
    {
        var isValidPhoneNumber = PhoneNumberHelpers.IsValidIranianMobileNumber(request.PhoneNumber);
        if (!isValidPhoneNumber)
        {
            throw new Exception("Phone number is not valid!");
        }


        var otpCode = "522368"; // TODO: use it in prod: GenerateOtpCode()
        var result = new SendOtpResponse();
        result.IsSMSSent =
            true; // TODO: use it in prod: result.IsSMSSent = await _otpService.SendOTPAsync(request.PhoneNumber, otpCode);

        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        var isUserExist = user != null;

        if (isUserExist)
        {
            result.IsNewUser = false;
            var otp = OTP.Add(user.Id, otpCode);
            await CreateOtpAsync(otp);
        }
        else
        {
            var newUser = new ApplicationUser { UserName = request.PhoneNumber, PhoneNumber = request.PhoneNumber };


            var createNewUserResult = await _userManager.CreateAsync(newUser);

            if (!createNewUserResult.Succeeded)
            {
                throw new Exception("Unable to insert user into the database.");
            }

            await AssignRoleToUserAsync(newUser, "Client");

            result.IsNewUser = true;

            var otp = OTP.Add(newUser.Id, otpCode);
            await CreateOtpAsync(otp);
        }

        return result;
    }


    public async Task<VerifyOtpResponse> VerifyOTPAndGenerateTokenAsync(VerifyOtpRequest request)
    {
        var isValidPhoneNumber = PhoneNumberHelpers.IsValidIranianMobileNumber(request.PhoneNumber);
        if (!isValidPhoneNumber)
        {
            throw new Exception("Phone number is not valid!");
        }

        var result = new VerifyOtpResponse();

        // Find the user by phone number
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (user.IsActive == false)
        {
            user.Activate();
        }

        // Query the pending OTP for the user
        var pendingOtp = await _queryDbContext.OTPs
            .Where(otp => otp.Status == OTPStatus.Pending)
            .OrderByDescending(otp => otp.CreatedAt)
            .FirstOrDefaultAsync(otp => otp.UserId == user.Id);

        if (pendingOtp == null)
        {
            throw new Exception("No pending OTP found for this phone number to verify!");
        }

        // Check if the OTP is expired
        var isTokenExpired = pendingOtp.CreatedAt.AddMinutes(int.Parse(_expireTimeMinutes)) < DateTime.UtcNow;
        if (isTokenExpired)
        {
            throw new Exception("OTP has expired!");
        }

        // Check if the OTP code is valid
        var isOTPValid = pendingOtp.OtpCode == request.OtpCode;
        if (!isOTPValid)
        {
            throw new Exception("Invalid OTP code.");
        }

        // Mark OTP as verified
        pendingOtp.Verify();
        _commandDbContext.Update(pendingOtp);

        // Get the latest term
        var term = await _queryDbContext.Terms.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
        if (term == null)
        {
            throw new Exception("No terms found in the system.");
        }


        var userTerm = ApplicationUserTerm.Add(user.Id, term.Id);
        _commandDbContext.ApplicationUserTerms.Add(userTerm);

        await _commandDbContext.SaveChangesAsync();


        var token = await _tokenService.GenerateTokenAsync(user);
        result.Token = token;
        result.PhoneNumber = user.PhoneNumber;

        if (user.FristName != null || user.LastName != null)
        {
            result.Name = $"{user.FristName} {user.LastName}";
        }
        else
        {
            result.Name = null;
        }


        return result;
    }


    public async Task<bool> LogoutAsync(LogoutRequest request)
    {
        
        // Extract user details from the token
        var userDetails = _tokenService.GetUserDetailsFromToken(request.Token);
        if (userDetails is null)
        {
            _logger.LogWarning("Logout failed: Invalid or expired token.");
            throw new Exception("Invalid or expired token.");   
        }

        // Get user ID from the claims
        var userId = userDetails.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Logout failed: User ID not found in token.");
           throw new Exception("User ID not found in token.");    
        }

        // Convert userId to int (if necessary)
        if (!int.TryParse(userId, out int parsedUserId))
        {
            _logger.LogWarning("Logout failed: User ID format is invalid.");
            throw new Exception("User ID format is invalid.");
        }

    
        // Remove refresh tokens (if applicable)
        var userTokens = _commandDbContext.UserTokens.Where(t => t.UserId == parsedUserId);
        _commandDbContext.UserTokens.RemoveRange(userTokens);

        // Save changes to persist logout actions
        await _commandDbContext.SaveChangesAsync();

        _logger.LogInformation($"User {parsedUserId} successfully logged out.");
        return true;
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


    private async Task CreateOtpAsync(OTP otp)
    {
        _commandDbContext.OTPs.Add(otp);
        await _commandDbContext.SaveChangesAsync();
    }

    private string GenerateOtpCode()
    {
        // Generate a random OTP code (e.g., 6-digit number)
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}