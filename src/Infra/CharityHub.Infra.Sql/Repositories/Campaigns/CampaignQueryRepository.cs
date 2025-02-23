namespace CharityHub.Infra.Sql.Repositories.Campaigns;

using CharityHub.Core.Contract.Primitives.Models;
using CharityHub.Core.Domain.Entities;
using CharityHub.Infra.Sql.Data.DbContexts;

using Core.Contract.Campaigns.Queries;
using Core.Contract.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Campaigns.Queries.GetCampaignById;
using Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public class CampaignQueryRepository(
    CharityHubQueryDbContext queryDbContext,
    ILogger<CampaignQueryRepository> logger,
    IHttpContextAccessor httpContextAccessor)
    : QueryRepository<Campaign>(queryDbContext), ICampaignQueryRepository
{
    public async Task<PagedData<CampaignsByCharityIdResponseDto>> GetCampaignsByCharityId(
        GetCampaignsByCharityIdQuery query)
    {
        #region Base URL Detection

        string baseUrl =
            $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

        #endregion

        #region Fetch Total Count

        var totalCount = await _queryDbContext.Campaigns
            .Where(c => c.CharityId == query.Id)
            .CountAsync();

        #endregion

        #region Fetch Paginated Campaigns with Logo & Banner

        var campaignsQuery = from campaign in _queryDbContext.Campaigns
            where campaign.CharityId == query.Id
            orderby campaign.StartDate
            join bannerFile in _queryDbContext.StoredFiles
                on campaign.BannerId equals bannerFile.Id into bannerFileGroup
            from bannerFile in bannerFileGroup.DefaultIfEmpty()
            select new CampaignsByCharityIdResponseDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                TotalAmount = campaign.TotalAmount,
                ChargedAmount = campaign.ChargedAmount,
                CharityName = campaign.Charity.Name,
                ChargedAmountProgressPercentage = (campaign.TotalAmount > 0)
                    ? (campaign.ChargedAmount / campaign.TotalAmount * 100)
                    : 0,
                CampaignStatus = campaign.CampaignStatus,


                // Campaign Banner
                BannerUriAddress = bannerFile != null
                    ? $"{baseUrl}{bannerFile.FilePath.Replace("\\", "/")}"
                    : $"{baseUrl}/uploads/default-campaign.png"
            };

        var campaigns = await campaignsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync();

        #endregion

        return new PagedData<CampaignsByCharityIdResponseDto>(campaigns, totalCount, query.PageSize, query.Page);
    }


    public async Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query)
    {
        var totalCount = await _queryDbContext.Campaigns.CountAsync();

        #region Base URL Detection

        string baseUrl =
            $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

        #endregion
        
        var campaignsQuery = from campaign in _queryDbContext.Campaigns
                .Include(c => c.Charity)
            where campaign.EndDate >= DateTime.UtcNow // Optional: Filter active campaigns
            orderby campaign.StartDate
            join bannerFile in _queryDbContext.StoredFiles
                on campaign.BannerId equals bannerFile.Id into bannerFileGroup
            from bannerFile in bannerFileGroup.DefaultIfEmpty()
            
            select new AllCampaignResponseDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                CharityName = campaign.Charity.Name,
                RemainingDayCount = CalculateRemainingDays(campaign.EndDate),
                ContributionAmount = campaign.ChargedAmount,
                TotalAmount = campaign.TotalAmount,
                ProgressPercentage = campaign.TotalAmount > 0
                    ? (campaign.ChargedAmount / campaign.TotalAmount) * 100
                    : 0,
                CampaignStatus = campaign.CampaignStatus,

                
                BannerUriAddress = bannerFile != null
                    ? $"{baseUrl}{bannerFile.FilePath.Replace("\\", "/")}"
                    : $"{baseUrl}/uploads/default-campaign.png"
            };

        var campaigns = await campaignsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync();

        return new PagedData<AllCampaignResponseDto>(campaigns, totalCount, query.PageSize, query.Page);
    }



    public async Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query)
    {
        // Fetch campaign details including the related charity
        var campaign = await _queryDbContext.Campaigns
                           .Include(c => c.Charity)
                           .FirstOrDefaultAsync(c => c.Id == query.Id)
                       ?? throw new KeyNotFoundException($"Campaign with ID {query.Id} not found.");

        // Fetch the number of unique contributors
        var contributorCount = await _queryDbContext.Transactions
            .Where(t => t.CampaignId == campaign.Id)
            .Select(t => t.UserId)
            .Distinct()
            .CountAsync();

        // Fetch the banner image if available
        var bannerFile = await _queryDbContext.StoredFiles
            .Where(f => f.Id == campaign.BannerId)
            .FirstOrDefaultAsync();

        var baseUrl = $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

        // Map to the DTO
        var campaignDto = new CampaignByIdResponseDto
        {
            Id = campaign.Id,
            Title = campaign.Title,
            Description = campaign.Description,
            ContributionAmount = contributorCount,
            RemainingDayCount = CalculateRemainingDays(campaign.EndDate),
            StartDateTime = campaign.StartDate,
            EndDateTime = campaign.EndDate,
            ChargedAmountProgressPercentage = (campaign.TotalAmount > 0) 
                ? (campaign.ChargedAmount / campaign.TotalAmount * 100) 
                : 0, // Safe division
            ChargedAmount = campaign.ChargedAmount,
            TotalAmount = campaign.TotalAmount,
            CharityName = campaign.Charity.Name,
            CharityId = campaign.CharityId,
            CampaignStatus = campaign.CampaignStatus,

            // Campaign Banner
            BannerUriAddress = bannerFile != null 
                ? $"{baseUrl}{bannerFile.FilePath.Replace("\\", "/")}" 
                : $"{baseUrl}/uploads/default-campaign.png"
        };

        return campaignDto;
    }



    private static int CalculateRemainingDays(DateTime endDate)
    {
        var now = DateTime.UtcNow; // Use provided date or default to UTC now
        var remainingDays = (endDate - now).TotalDays;

        return remainingDays > 0 ? (int)Math.Ceiling(remainingDays) : 0; // Ensure no negative values
    }
}