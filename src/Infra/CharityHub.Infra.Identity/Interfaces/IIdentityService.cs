namespace CharityHub.Infra.Identity.Interfaces;


public interface IIdentityService
{
    Task<bool> SendOTPAsync(string phoneNumber, bool acceptedTerms);
    Task<string> VerifyOTPAndGenerateTokenAsync(string phoneNumber, string otp);
}
