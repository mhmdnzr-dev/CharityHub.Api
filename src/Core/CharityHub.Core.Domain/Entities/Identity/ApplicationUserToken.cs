namespace CharityHub.Core.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;

public class ApplicationUserToken : IdentityUserToken<int>
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
