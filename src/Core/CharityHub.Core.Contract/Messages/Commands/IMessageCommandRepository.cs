namespace CharityHub.Core.Contract.Messages.Commands;

using Domain.Entities;

using Primitives.Repositories;

using SeenMessage;

public interface IMessageCommandRepository : ICommandRepository<Message>
{

   
}