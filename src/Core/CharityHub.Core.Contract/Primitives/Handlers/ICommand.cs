using MediatR;

namespace CharityHub.Core.Contract.Primitives.Handlers;

public interface ICommand : IRequest<int>
{
    // Command marker interface with predefined return type
}
