namespace CharityHub.Infra.Identity.Models.Token.Responses;

public class GenerateTokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}