namespace CharityHub.Infra.Identity.Models;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ProfileResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string PhoneNumber { get; set; }
}