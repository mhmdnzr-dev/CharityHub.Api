namespace CharityHub.Infra.Identity.Models;
public class SendOtpResponse
{
    public bool IsNewUser { get; set; }
    public bool IsSMSSent { get; set; }
}
