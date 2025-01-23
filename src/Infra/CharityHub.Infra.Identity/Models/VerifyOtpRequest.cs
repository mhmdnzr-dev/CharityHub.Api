namespace CharityHub.Infra.Identity.Models;
public class VerifyOtpRequest
{
    public string PhoneNumber { get; set; }
    public string Otp { get; set; }
    public bool AcceptedTerms { get; set; }
}
