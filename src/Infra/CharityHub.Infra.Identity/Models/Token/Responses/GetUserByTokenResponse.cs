namespace CharityHub.Infra.Identity.Models.Token.Responses;

using System.Security.Claims;

public class GetUserByTokenResponse
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string PhoneNumber { get; set; }
 
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string Issuer { get; set; }
    public string? Audience { get; set; }
    public DateTime Expiration { get; set; }

    public ClaimsPrincipal ClaimsPrincipal { get; set; }
}