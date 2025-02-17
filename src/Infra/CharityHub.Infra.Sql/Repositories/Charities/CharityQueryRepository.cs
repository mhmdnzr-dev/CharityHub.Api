namespace CharityHub.Infra.Sql.Repositories.Charities;

using Core.Contract.Charity.Queries;
using Core.Contract.Charity.Queries.GetAllCharities;
using Core.Contract.Charity.Queries.GetCharityById;
using Core.Contract.Primitives.Models;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Core.Contract.Configuration.Models;

using Microsoft.EntityFrameworkCore;

using Primitives;

public class CharityQueryRepository(
    CharityHubQueryDbContext queryDbContext,
    ILogger<CharityQueryRepository> logger,
    IOptions<FileOptions> options)
    : QueryRepository<Charity>(queryDbContext), ICharityQueryRepository
{
    public async Task<PagedData<AllCharitiesResponseDto>> GetAllAsync(GetAllCharitiesQuery query)
    {
        #region Query Data

        var charities = _queryDbContext.Charities
            .Where(c => c.IsActive)
            .AsQueryable();

        var campaigns = _queryDbContext.Campaigns.AsQueryable();
        var storedFiles = _queryDbContext.StoredFiles.AsQueryable();

        #endregion

        #region Result Query

        var resultQuery = from charity in charities
            join campaign in campaigns
                on charity.Id equals campaign.CharityId into campaignGroup
            from campaign in campaignGroup.DefaultIfEmpty()
            join storedFile in storedFiles
                on charity.LogoId equals storedFile.Id into fileGroup
            from storedFile in fileGroup.DefaultIfEmpty()
            group new { campaign, storedFile } by new { charity.Id, charity.Name, storedFile.FilePath }
            into charityGroup
            select new AllCharitiesResponseDto
            {
                Id = charityGroup.Key.Id,
                Name = charityGroup.Key.Name,
                CampaignCount = charityGroup.Count(c => c.campaign != null),
                PhotoUriAddress = charityGroup.Key.FilePath != null
                    ? $"{options.Value.UploadDirectory}/{charityGroup.Key.FilePath.Replace("\\", "/")}" // Convert backslashes to forward slashes
                    : $"{options.Value.UploadDirectory}/default-image.png"
            };

        #endregion

        #region Pagination

        var totalCount = resultQuery.Count();

        var paginatedResult = await resultQuery
            .OrderBy(c => c.Name)
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