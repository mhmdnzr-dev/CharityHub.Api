namespace CharityHub.Infra.Sql.Repositories.Messages;

using Core.Contract.Features.Messages.Commands;


using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public class MessageCommandRepository(
    CharityHubCommandDbContext commandDbContext,
    ILogger<MessageCommandRepository> _logger)
    : CommandRepository<Message>(commandDbContext), IMessageCommandRepository
{

}