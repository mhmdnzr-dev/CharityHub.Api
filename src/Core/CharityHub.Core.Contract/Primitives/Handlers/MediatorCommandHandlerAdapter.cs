using MediatR;

namespace CharityHub.Core.Contract.Primitives.Handlers;

public class MediatorCommandHandlerAdapter<TCommand> : IRequestHandler<TCommand, int>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;

    public MediatorCommandHandlerAdapter(ICommandHandler<TCommand> handler)
    {
        _handler = handler;
    }

    public async Task<int> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await _handler.Handle(request, cancellationToken);
    }
}
