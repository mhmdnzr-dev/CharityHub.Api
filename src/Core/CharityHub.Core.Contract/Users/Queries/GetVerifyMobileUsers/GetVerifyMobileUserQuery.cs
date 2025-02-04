namespace CharityHub.Core.Contract.Users.Queries.GetVerifyMobileUsers;

using CharityHub.Core.Contract.Primitives.Handlers;

public class GetVerifyMobileUserQuery : IQuery<VerifyMobileUserResponseDto>
{
    public string PhoneNumber { get; set; }
    public string OTPCode { get; set; }
}