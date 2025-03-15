namespace CharityHub.Infra.Sql.Repositories.Charities;

using Core.Contract.Features.Charity.Commands;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.Extensions.Logging;

using Primitives;

public class CharityCommandRepository(CharityHubCommandDbContext queryDbContext, ILogger<CharityCommandRepository> logger)
    : CommandRepository<Charity>(queryDbContext), ICharityCommandRepository
{
    
}