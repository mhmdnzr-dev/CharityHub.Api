namespace CharityHub.Infra.Identity.Interfaces;
using System.Threading.Tasks;

public interface IOTPService
{
    Task<bool> SendOtpAsync(string phoneNumber, string otp);
}
