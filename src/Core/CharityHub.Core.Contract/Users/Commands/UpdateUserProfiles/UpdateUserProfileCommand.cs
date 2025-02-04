namespace CharityHub.Core.Contract.Users.Commands.UpdateUserProfiles;

using Primitives.Handlers;

public class UpdateUserProfileCommand : ICommand<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}