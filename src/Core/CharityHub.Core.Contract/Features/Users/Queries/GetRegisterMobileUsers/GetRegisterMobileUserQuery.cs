namespace CharityHub.Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;

using CharityHub.Core.Contract.Primitives.Handlers;

public class GetRegisterMobileUserQuery : IQuery<RegisterMobileUserResponseDto>
{
    public string PhoneNumber { get; set; }
}