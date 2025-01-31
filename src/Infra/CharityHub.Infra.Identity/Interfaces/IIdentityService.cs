namespace CharityHub.Infra.Identity.Interfaces;

using CharityHub.Infra.Identity.Models;

public interface IIdentityService
{
    Task<SendOtpResponse> SendOTPAsync(SendOtpRequest request);
    Task<VerifyOtpResponse> VerifyOTPAndGenerateTokenAsync(VerifyOtpRequest request);
    Task<bool> LogoutAsync(LogoutRequest request);
}
