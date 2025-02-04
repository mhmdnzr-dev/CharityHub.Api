namespace CharityHub.Core.Contract.Primitives.Handlers;
using MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
