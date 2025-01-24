
using Microsoft.AspNetCore.Identity;
namespace CharityHub.Core.Domain.Entities.Identity;
public sealed class ApplicationUser : IdentityUser<int>
{
    public bool IsActive { get; private set; }
    public ICollection<Charity> Charities { get; private set; } = new HashSet<Charity>();



    public ApplicationUser() : base()
    {
    }


    public void Activate()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("User is already active.");
        }

        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User is already inactive.");
        }

        IsActive = false;
    }
}
