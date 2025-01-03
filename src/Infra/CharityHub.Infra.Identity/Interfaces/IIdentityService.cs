using CharityHub.Core.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;
namespace CharityHub.Infra.Identity.Interfaces;


public interface IIdentityService
{
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string role);
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string role);
    Task<IdentityResult> CreateRoleAsync(string roleName);
    Task<ApplicationUser> FindUserByEmailAsync(string email);
    Task<ApplicationUser> FindUserByIdAsync(string userId);
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
    Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);
    IEnumerable<IdentityRole> GetAllRoles();
}
