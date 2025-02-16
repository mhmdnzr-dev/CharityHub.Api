namespace CharityHub.Infra.Sql.Repositories.Messages;

using Core.Contract.Messages.Queries;
using Core.Contract.Messages.Queries.GetMessageByUserIdQuery;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;


public class MessageQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<MessageQueryRepository> logger)
    : QueryRepository<Message>(queryDbContext), IMessageQueryRepository
{
    public async Task<IEnumerable<MessageByUserIdDto>> GetAllByUserId(int userId)
    {
        var query = _queryDbContext.Messages.AsQueryable();

        query = query.Where(x => x.UserId == userId);

        var result = await query.Select(x =>
                new MessageByUserIdDto { IsSeen = x.IsSeen, SeenDate = x.SeenDateTime, Content = x.Content, })
            .ToArrayAsync();
        return result;
    }
}