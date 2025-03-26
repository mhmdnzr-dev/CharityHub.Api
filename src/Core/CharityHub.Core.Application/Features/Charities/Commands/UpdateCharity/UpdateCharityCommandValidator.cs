namespace CharityHub.Core.Application.Features.Charities.Commands.UpdateCharity;

using Contract.Features.Charity.Commands.UpdateCharity;

using FluentValidation;

public class UpdateCharityCommandValidator 
//     : AbstractValidator<UpdateCharityCommand>
 {
//     public UpdateCharityCommandValidator()
//     {
//         RuleFor(x => x.Id)
//             .GreaterThan(0);
//
//         RuleFor(x => x.Name)
//             .NotEmpty()
//             .MaximumLength(200);
//
//         RuleFor(x => x.Website)
//             .MaximumLength(500)
//             .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrWhiteSpace(x.Website));
//
//         RuleFor(x => x.CityId)
//             .GreaterThan(0);
//
//         RuleFor(x => x.ContactName)
//             .NotEmpty()
//             .MaximumLength(100);
//
//         RuleFor(x => x.ContactPhone)
//             .Matches(@"^\+?[1-9]\d{1,14}$")
//             .When(x => !string.IsNullOrWhiteSpace(x.ContactPhone));
//
//         RuleFor(x => x.Telephone)
//             .Matches(@"^\+?[0-9]{7,15}$")
//             .When(x => !string.IsNullOrWhiteSpace(x.Telephone));
//
//         RuleFor(x => x.ManagerName)
//             .MaximumLength(100);
//
//         RuleFor(x => x.Address)
//             .MaximumLength(500);
//
//         RuleFor(x => x.Description)
//             .MaximumLength(1000);
//
//         RuleFor(x => x.BannerBase64)
//             .Must(b => b != null && b.Length > 0).When(x => x.BannerBase64 != null)
//             .WithMessage("Invalid Banner file.");
//
//         RuleFor(x => x.LogoBase64)
//             .Must(b => b != null && b.Length > 0).When(x => x.LogoBase64 != null)
//             .WithMessage("Invalid Logo file.");
//
//         RuleFor(x => x.BannerFileExtension)
//             .Must(IsValidImageExtension).When(x => !string.IsNullOrWhiteSpace(x.BannerFileExtension));
//
//         RuleFor(x => x.LogoFileExtension)
//             .Must(IsValidImageExtension).When(x => !string.IsNullOrWhiteSpace(x.LogoFileExtension));
//     }
//
//     private bool IsBase64String(string value)
//     {
//         return Convert.TryFromBase64String(value, new Span<byte>(new byte[value.Length]), out _);
//     }
//
//     private bool IsValidImageExtension(string extension)
//     {
//         var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
//         return allowedExtensions.Contains(extension.ToLower());
//     }
}