namespace CharityHub.Infra.Identity.Models;
public class VerifyOtpResponse
{
    public string Token { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
}

// dto response schema in swagger
