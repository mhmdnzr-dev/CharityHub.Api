namespace CharityHub.Core.Application.Features.Users.Queries.GetRegisterMobileUsers;

using Contract.Features.Users.Queries.GetRegisterMobileUsers;

using FluentValidation;

public class GetRegisterUserQueryValidator : AbstractValidator<GetRegisterMobileUserQuery>
{
    public GetRegisterUserQueryValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^0[9]\d{9}$")
            .WithMessage(
                "Phone number must be a valid Iranian mobile number, starting with 09 and followed by 9 digits.");
    }
}