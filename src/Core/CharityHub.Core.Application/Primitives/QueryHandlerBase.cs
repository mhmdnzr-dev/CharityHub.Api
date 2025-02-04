namespace CharityHub.Core.Application.Primitives;

using Contract.Primitives.Handlers;

using Microsoft.Extensions.Caching.Memory;

public abstract class QueryHandlerBase<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    protected readonly IMemoryCache Cache;

    protected QueryHandlerBase(IMemoryCache cache)
    {
        Cache = cache;
    }

    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}
