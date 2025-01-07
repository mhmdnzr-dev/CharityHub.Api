namespace CharityHub.Core.Contract.Primitives;
using MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    // Marker interface for queries with a response type
}