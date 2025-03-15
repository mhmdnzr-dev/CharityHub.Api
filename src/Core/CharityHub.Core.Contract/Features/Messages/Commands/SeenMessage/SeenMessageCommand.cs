namespace CharityHub.Core.Contract.Features.Messages.Commands.SeenMessage;

using Primitives.Handlers;

public class SeenMessageCommand : ICommand<int>
{
    public int Id { get; set; }
}