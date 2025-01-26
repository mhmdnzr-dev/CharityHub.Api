namespace CharityHub.Infra.Sql.Repositories.Charities;

using Core.Contract.Charity.Queries;
using Core.Contract.Charity.Queries.GetAllCharities;
using Core.Contract.Charity.Queries.GetCharityById;
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
            group campaign by new { charity.Id, charity.Name }
            into charityGroup
            select new AllCharitiesResponseDto
            {
                Id = charityGroup.Key.Id,
                Name = charityGroup.Key.Name,
                CampaignCount = charityGroup.Count(c => c != null), // Count campaigns
                PhotoUriAddress = "https://picsum.photos/500"
            };

        #endregion

        // Materialize the result into a list
        return await result.ToArrayAsync();
    }


    public async Task<CharityByIdResponseDto> GetDetailedById(GetCharityByIdQuery query)
    {
        var result = await (from charity in _queryDbContext.Charities
            join social in _queryDbContext.Socials
                on charity.SocialId equals social.Id into socialsGroup
            from social in socialsGroup.DefaultIfEmpty() // Left join
            where charity.Id == query.Id
            select new { Charity = charity, Socials = socialsGroup }).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new KeyNotFoundException($"Charity with ID {query.Id} not found.");
        }

        var socialsList = result.Socials.Select(s => new SocialModel { Name = s.Name, Url = "https://test.com" })
            .ToList();
        var response = new CharityByIdResponseDto
        {
            Name = result.Charity.Name, Description = result.Charity.Description, Socials = socialsList
        };

        return response;
    }
}