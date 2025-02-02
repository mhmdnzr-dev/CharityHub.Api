
using Microsoft.AspNetCore.Identity;
namespace CharityHub.Core.Domain.Entities.Identity;
public sealed class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public bool IsActive { get; private set; }
    
    
    private readonly List<Charity> _charities = new();
    public IReadOnlyCollection<Charity> Charities => _charities.AsReadOnly();
    


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
