namespace CharityHub.Core.Contract.Features.Users.Commands.UpdateUserProfiles;

using Primitives.Handlers;

public class UpdateUserProfileCommand : ICommand<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}