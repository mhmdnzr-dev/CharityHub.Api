namespace CharityHub.Infra.Identity.Interfaces;
using System.Threading.Tasks;

public interface IOTPService
{
    Task<bool> SendOTPAsync(string phoneNumber, string otp);
}
