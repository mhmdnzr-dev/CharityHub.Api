namespace CharityHub.Core.Application.Configuration.Models;
public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public string EpireMinute { get; set; }
}
