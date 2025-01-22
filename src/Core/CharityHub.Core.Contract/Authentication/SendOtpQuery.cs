namespace CharityHub.Core.Contract.Authentication;
public class SendOtpQuery
{
    public required string PhoneNumber { get; set; }
    public bool AcceptedTerms { get; set; }
}
