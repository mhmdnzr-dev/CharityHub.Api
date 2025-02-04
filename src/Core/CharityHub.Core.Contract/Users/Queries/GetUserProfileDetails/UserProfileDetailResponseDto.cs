namespace CharityHub.Core.Contract.Users.Queries.GetUserProfileDetails;

public class UserProfileDetailResponseDto
{
    public string? UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
}