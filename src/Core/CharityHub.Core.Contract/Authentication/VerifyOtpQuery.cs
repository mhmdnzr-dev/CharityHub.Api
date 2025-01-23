namespace CharityHub.Core.Contract.Authentication;
public class VerifyOtpQuery
{
    public string PhoneNumber { get; set; }
    public string Otp { get; set; }
    public bool AcceptedTerms { get; set; }
}
