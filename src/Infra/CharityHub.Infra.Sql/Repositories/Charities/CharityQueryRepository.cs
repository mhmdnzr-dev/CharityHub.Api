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

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Primitives;

public class CharityQueryRepository(
    CharityHubQueryDbContext queryDbContext,
    ILogger<CharityQueryRepository> logger,
    IOptions<FileOptions> options,
    IHttpContextAccessor httpContextAccessor) 
    : QueryRepository<Charity>(queryDbContext), ICharityQueryRepository
{
    public async Task<PagedData<AllCharitiesResponseDto>> GetAllAsync(GetAllCharitiesQuery query)
    {
        #region Base URL Detection

        string baseUrl =
            $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

        #endregion

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
                    ? $"{baseUrl}{charityGroup.Key.FilePath.Replace("\\", "/")}" // Ensure correct URL
                    : $"{baseUrl}/uploads/default-charity.png"
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


    public async Task<CharityByIdResponseDto> GetDetailedById(GetCharityByIdQuery dto)
    {
        #region Base URL Detection

        string baseUrl =
            $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

        #endregion

        #region Query Charity with Logo & Banner

        var query = await (from charity in _queryDbContext.Charities
            where charity.Id == dto.Id && charity.IsActive
            join storedLogoFile in _queryDbContext.StoredFiles
                on charity.LogoId equals storedLogoFile.Id into logoFileGroup
            from storedLogoFile in logoFileGroup.DefaultIfEmpty()
            join storedBannerFile in _queryDbContext.StoredFiles
                on charity.BannerId equals storedBannerFile.Id into bannerFileGroup
            from storedBannerFile in bannerFileGroup.DefaultIfEmpty()
            select new
            {
                Charity = charity,
                LogoFilePath = storedLogoFile.FilePath,
                BannerFilePath = storedBannerFile.FilePath
            }).FirstOrDefaultAsync();

        #endregion

        if (query is null)
            return new CharityByIdResponseDto();

        #region Fetch Socials Separately

        var socials = await _queryDbContext.Socials
            .Where(s => s.Id == query.Charity.SocialId)
            .Select(s => new SocialDtoModel { Name = s.Name, Url = s.Url })
            .ToListAsync();

        #endregion

        #region Construct Result

        var result = new CharityByIdResponseDto
        {
            Name = query.Charity.Name,
            Description = query.Charity.Description,
            PhotoUriAddress = !string.IsNullOrEmpty(query.LogoFilePath)
                ? $"{baseUrl}{query.LogoFilePath.Replace("\\", "/")}"
                : $"{baseUrl}/uploads/default-charity.png",
            BannerUriAddress = !string.IsNullOrEmpty(query.BannerFilePath)
                ? $"{baseUrl}{query.BannerFilePath.Replace("\\", "/")}"
                : $"{baseUrl}/uploads/default-banner.png",
            Socials = socials
        };

        #endregion

        return result;
    }
}