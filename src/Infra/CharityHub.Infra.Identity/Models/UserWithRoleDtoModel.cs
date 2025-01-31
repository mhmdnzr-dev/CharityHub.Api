namespace CharityHub.Infra.Identity.Models;

public class UserWithRolesDtoModel
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string PhoneNumber { get; set; }
    public List<string>? Roles { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
}
