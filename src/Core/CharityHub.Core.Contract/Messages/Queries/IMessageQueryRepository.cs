namespace CharityHub.Core.Contract.Messages.Queries;

using Domain.Entities;

using GetMessageByUserIdQuery;

using Primitives.Repositories;

public interface IMessageQueryRepository : IQueryRepository<Message>
{
    Task<IEnumerable<MessageByUserIdDto>> GetAllByUserId(int userId);
}