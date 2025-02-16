namespace CharityHub.Infra.Sql.Repositories.Messages;

using Core.Contract.Messages.Commands;
using Core.Contract.Messages.Commands.SeenMessage;
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