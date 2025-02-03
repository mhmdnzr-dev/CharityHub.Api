namespace CharityHub.Core.Contract.Primitives.Handlers;

using MediatR;

public interface IQueryHandler<TQuery, TResponse> 
    : IRequestHandler<TQuery, TResponse> 
    where TQuery : IQuery<TResponse> { }
