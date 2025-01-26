namespace CharityHub.Infra.Sql.Repositories.Charities;

using Core.Contract.Charity.Queries;
using Core.Contract.Charity.Queries.GetAllCharities;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Premitives;

public class CharityQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<CharityQueryRepository> logger)
    : QueryRepository<Charity>(queryDbContext), ICharityQueryRepository
{
    public async Task<IEnumerable<AllCharitiesResponseDto>> GetAllAsync(GetAllCharitiesQuery query)
    {
        #region Query Data
        
        var charities = _queryDbContext.Charities.AsQueryable();
        var campaigns = _queryDbContext.Campaigns.AsQueryable();

        #endregion

        #region Filter Active Charities
        
        if (charities.Any())
        {
            charities = charities.Where(c => c.IsActive);
        }

        #endregion

        #region Result

        
        var result = from charity in charities
            join campaign in campaigns
                on charity.Id equals campaign.CharityId into campaignGroup // Left join
            from campaign in campaignGroup.DefaultIfEmpty() // Include charities without campaigns
            group campaign by new { charity.Id, charity.Name, charity.PhotoUriAddress }
            into charityGroup
            select new AllCharitiesResponseDto
            {
                Id = charityGroup.Key.Id,
                Name = charityGroup.Key.Name,
                CampaignCount = charityGroup.Count(c => c != null), // Count campaigns
                PhotoUriAddress = charityGroup.Key.PhotoUriAddress
            };

        #endregion

        // Execute and return the result as an array
        return await result.ToArrayAsync();
    }
}