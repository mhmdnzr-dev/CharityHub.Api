using MediatR;

namespace CharityHub.Core.Contract.Primitives.Handlers;
public interface ICommand<out TResponse> : IRequest<TResponse>
{
    // Marker interface, can be extended with shared properties or methods
}

public interface ICommand : ICommand<int>
{
    // Default ICommand with int as response type
}