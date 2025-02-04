namespace CharityHub.Infra.Identity.Models.Identity.Requests;

public class UpdateProfileRequest
{
    public string Token { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}