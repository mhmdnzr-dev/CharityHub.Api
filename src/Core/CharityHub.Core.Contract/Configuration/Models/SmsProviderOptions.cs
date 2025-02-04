namespace CharityHub.Core.Contract.Configuration.Models;
public class SmsProviderOptions
{
    public string ApiKey { get; set; }
    public string SenderNumber { get; set; }
    public string ExpireMinute { get; set; }
}
