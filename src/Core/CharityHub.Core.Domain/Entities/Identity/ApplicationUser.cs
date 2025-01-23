
using Microsoft.AspNetCore.Identity;
namespace CharityHub.Core.Domain.Entities.Identity;
public class ApplicationUser : IdentityUser
{
    public string? OTP { get; set; }
    public DateTime OTPCreationTime { get; set; }

    public ICollection<Charity> Charities { get; set; } = new HashSet<Charity>();
}
