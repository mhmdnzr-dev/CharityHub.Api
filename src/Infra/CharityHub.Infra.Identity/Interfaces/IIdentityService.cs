namespace CharityHub.Infra.Identity.Interfaces;

using CharityHub.Infra.Identity.Models;

using Models.Identity.Requests;

public interface IIdentityService
{
    Task<SendOtpResponse> SendOTPAsync(SendOtpRequest request);
    Task<VerifyOtpResponse> VerifyOTPAndGenerateTokenAsync(VerifyOtpRequest request);
    Task<ProfileResponse> GetUserProfileByToken(ProfileRequest request);
    Task<bool> UpdateProfileAsync(UpateProfileRequest request, string token);
    Task<bool> LogoutAsync(LogoutRequest request);
}