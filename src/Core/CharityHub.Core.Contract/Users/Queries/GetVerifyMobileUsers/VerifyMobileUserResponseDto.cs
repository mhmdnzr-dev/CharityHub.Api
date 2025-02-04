namespace CharityHub.Core.Contract.Users.Queries.GetVerifyMobileUsers;

public class VerifyMobileUserResponseDto
{
    public string Token { get; set; }
    public string? Name { get; set; }
    public string PhoneNumber { get; set; }
}