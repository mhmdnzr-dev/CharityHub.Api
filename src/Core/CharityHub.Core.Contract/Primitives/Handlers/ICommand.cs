using MediatR;

namespace CharityHub.Core.Contract.Primitives.Handlers;

public interface ICommand<TResponse> : IRequest<TResponse> { }
