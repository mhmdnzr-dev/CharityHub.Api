using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace CharityHub.Infra.Identity.Services;


public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Create a new user
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    // Assign a role to a user
    public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    // Get user roles
    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    // Remove a user from a role
    public async Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string role)
    {
        return await _userManager.RemoveFromRoleAsync(user, role);
    }

    // Create a new role
    public async Task<IdentityResult> CreateRoleAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole(roleName);
            return await _roleManager.CreateAsync(role);
        }
        return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
    }

    // Find a user by email
    public async Task<ApplicationUser> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    // Find a user by ID
    public async Task<ApplicationUser> FindUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    // Update a user
    public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

    // Delete a user
    public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
    {
        return await _userManager.DeleteAsync(user);
    }

    // Check if a user is in a specific role
    public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
    {
        return await _userManager.IsInRoleAsync(user, role);
    }

    // Get all roles
    public IEnumerable<IdentityRole> GetAllRoles()
    {
        return _roleManager.Roles;
    }
}
