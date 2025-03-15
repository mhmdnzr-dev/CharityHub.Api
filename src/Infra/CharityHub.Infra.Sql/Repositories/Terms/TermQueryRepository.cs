namespace CharityHub.Infra.Sql.Repositories.Terms;

using System.Threading.Tasks;


using Core.Contract.Features.Terms.Queries;
using Core.Contract.Features.Terms.Queries.GetLastTerm;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public sealed class TermQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<TermQueryRepository> logger)
    : QueryRepository<Term>(queryDbContext), ITermQueryRepository
{
    public async Task<LastTermResponseDto> GetLastTerm(GetLastTermQuery query)
    {
        logger.LogInformation("Fetching the last term from the database.");

        var term = await _queryDbContext.Terms.OrderByDescending(t => t.Id).FirstOrDefaultAsync();

        if (term == null)
        {
            logger.LogWarning("No terms found in the system.");
            throw new Exception("There is no term in system!");
        }

        var result = new LastTermResponseDto { Content = term.Description };
        logger.LogInformation("Successfully retrieved the last term.");
        return result;
    }
}
