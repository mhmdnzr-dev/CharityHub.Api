namespace CharityHub.Core.Contract.Features.Messages.Queries;

using Domain.Entities;

using GetMessageByUserIdQuery;

using Primitives.Repositories;

public interface IMessageQueryRepository : IQueryRepository<Message>
{
    Task<IEnumerable<MessageByUserIdDto>> GetAllByUserId(int userId);
}