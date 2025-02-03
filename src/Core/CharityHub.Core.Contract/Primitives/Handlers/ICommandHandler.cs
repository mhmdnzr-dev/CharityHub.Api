namespace CharityHub.Core.Contract.Primitives.Handlers;

using MediatR;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, int>
    where TCommand : ICommand
{
}

