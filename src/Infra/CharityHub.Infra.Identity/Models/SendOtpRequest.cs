namespace CharityHub.Infra.Identity.Models;
public class SendOtpRequest
{
    public string PhoneNumber { get; set; }
    public bool AcceptedTerm { get; set; }
}
