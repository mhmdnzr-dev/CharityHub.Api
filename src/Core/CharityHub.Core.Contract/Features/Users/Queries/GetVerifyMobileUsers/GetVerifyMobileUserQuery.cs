namespace CharityHub.Core.Contract.Features.Users.Queries.GetVerifyMobileUsers;

using Primitives.Handlers;

public class GetVerifyMobileUserQuery : IQuery<VerifyMobileUserResponseDto>
{
    public string PhoneNumber { get; set; }
    public string OTPCode { get; set; }
}