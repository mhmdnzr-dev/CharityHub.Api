namespace CharityHub.Infra.Identity.Models;
public class SendOtpResponse
{
    public int NewUserId { get; set; }
    public bool IsNewUser { get; set; }
    public bool IsSMSSent { get; set; }
}
