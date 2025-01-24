
using Microsoft.AspNetCore.Identity;
namespace CharityHub.Core.Domain.Entities.Identity;
public sealed class ApplicationUser : IdentityUser<int>
{
    public bool IsActive { get; set; }
    public ICollection<Charity> Charities { get; set; } = new HashSet<Charity>();



    public ApplicationUser() : base()
    {
    }
}
