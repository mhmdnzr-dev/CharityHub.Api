namespace CharityHub.Infra.Identity.Models.Identity.Responses;
public class VerifyOtpResponse
{
    public string Token { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
}

