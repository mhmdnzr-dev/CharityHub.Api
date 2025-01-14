namespace CharityHub.Infra.Identity.Interfaces;


public interface IIdentityService
{
    Task<bool> SendOTPAsync(string phoneNumber);
    Task<bool> VerifyOTPAsync(string phoneNumber, string otp);
}
