namespace CharityHub.Infra.Sql.Repositories.Charities;

using Core.Contract.Charity.Queries;
using Core.Contract.Charity.Queries.GetAllCharities;
using Core.Contract.Charity.Queries.GetCharityById;
using Core.Contract.Primitives.Models;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Premitives;

public class CharityQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<CharityQueryRepository> logger)
    : QueryRepository<Charity>(queryDbContext), ICharityQueryRepository
{
    public async Task<PagedData<AllCharitiesResponseDto>> GetAllAsync(GetAllCharitiesQuery query)
    {
        #region Query Data

        var charities = _queryDbContext.Charities
            .Where(c => c.IsActive) // Directly filter active charities
            .AsQueryable();

        var campaigns = _queryDbContext.Campaigns.AsQueryable();

        #endregion

        #region Result Query

        var resultQuery = from charity in charities
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

        #region Pagination

        var totalCount = await resultQuery.CountAsync(); // Get total count before pagination

        var paginatedResult = await resultQuery
            .OrderBy(c => c.Name) // Ensure consistent ordering
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync();

        #endregion

        return new PagedData<AllCharitiesResponseDto>(paginatedResult, totalCount, query.PageSize, query.Page);
    }


    public async Task<CharityByIdResponseDto> GetDetailedById(GetCharityByIdQuery query)
    {
        // Retrieve the charity and its associated socials using a LINQ query
        var result = await (from charity in _queryDbContext.Charities
            where charity.Id == query.Id
            join social in _queryDbContext.Socials
                on charity.SocialId equals social.Id into socialsGroup
            select new { Charity = charity, Socials = socialsGroup.ToList() }).FirstOrDefaultAsync();

        // If no result is found, return null or throw an exception based on your needs
        if (result == null)
            return new CharityByIdResponseDto();

        // Map the result to the response DTO
        var response = new CharityByIdResponseDto
        {
            Name = result.Charity.Name,
            Description = result.Charity.Description,
            Socials = result.Socials.Select(s => new SocialDtoModel { Name = s.Name, Url = "https://google.com" })
                .ToList()
        };

        return response;
    }
}