namespace CharityHub.Infra.Identity.Models;
public class VerifyOtpRequest
{
    public string PhoneNumber { get; set; }
    public string OtpCode { get; set; }
}
