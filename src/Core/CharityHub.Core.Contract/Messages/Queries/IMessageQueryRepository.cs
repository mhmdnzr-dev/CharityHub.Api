namespace CharityHub.Core.Contract.Messages.Queries;

using GetMessageByUserIdQuery;

public interface IMessageQueryRepository
{
    Task<IEnumerable<MessageByUserIdDto>> GetAllByUserId(int userId);
}