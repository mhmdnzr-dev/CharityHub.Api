using Microsoft.AspNetCore.Identity;
namespace CharityHub.Core.Domain.Entities.Identity;


public class ApplicationRole : IdentityRole
{
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public ApplicationRole() : base()
    {
    }
}