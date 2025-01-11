using MediatR;

namespace CharityHub.Core.Contract.Primitives.Handlers;


public class MediatorQueryHandlerAdapter<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _handler;

    public MediatorQueryHandlerAdapter(IQueryHandler<TQuery, TResponse> handler)
    {
        _handler = handler;
    }

    public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken)
    {
        return await _handler.Handle(query, cancellationToken);
    }
}