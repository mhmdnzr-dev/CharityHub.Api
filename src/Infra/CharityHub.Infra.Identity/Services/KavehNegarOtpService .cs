using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Infra.Identity.Interfaces;

using Kavenegar;

using Microsoft.Extensions.Options;


namespace CharityHub.Infra.Identity.Services;


public class KavenegarOtpService : IOTPService
{
    private readonly string _apiKey;
    private readonly string _sender;

    public KavenegarOtpService(IOptions<SmsProviderOptions> options)
    {
        _apiKey = options.Value.ApiKey;
        _sender = options.Value.SenderNumber;
    }

    public async Task<bool> SendOTPAsync(string phoneNumber, string otp)
    {
        try
        {
            var api = new KavenegarApi(_apiKey);
            var message = $"Your OTP is: {otp}";

            var test = await Task.Run(() => api.Send(_sender, phoneNumber, message));

            return true;
        }
        catch (Exception ex)
        {

            throw new Exception($"Failed to send OTP: {ex.Message}");
        }
    }
}