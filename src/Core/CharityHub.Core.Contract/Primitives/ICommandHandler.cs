
namespace CharityHub.Core.Contract.Primitives;


public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<int> Handle(TCommand command, CancellationToken cancellationToken);
}