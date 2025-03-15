namespace CharityHub.Infra.Sql.Repositories.Campaigns;

using CharityHub.Core.Contract.Primitives.Models;

using Core.Contract.Features.Campaigns.Queries;
using Core.Contract.Features.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Features.Campaigns.Queries.GetCampaignById;
using Core.Contract.Features.Campaigns.Queries.GetCampaignsByCharityId;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public class CampaignQueryRepository(
    CharityHubQueryDbContext queryDbContext,
    ILogger<CampaignQueryRepository> logger)
    : QueryRepository<Campaign>(queryDbContext), ICampaignQueryRepository
{
    public async Task<PagedData<CampaignsByCharityIdResponseDto>> GetCampaignsByCharityId(
        GetCampaignsByCharityIdQuery query)
    {
        #region Fetch Total Count

        var totalCount = await _queryDbContext.Campaigns
            .Where(c => c.CharityId == query.Id)
            .CountAsync();

        #endregion

        #region Fetch Paginated Campaigns with Logo & Banner

        var campaignsQuery = from campaign in _queryDbContext.Campaigns
            where campaign.CharityId == query.Id && campaign.IsActive
            orderby campaign.StartDate
            join bannerFile in _queryDbContext.StoredFiles
                on campaign.BannerId equals bannerFile.Id into bannerFileGroup
            from bannerFile in bannerFileGroup.DefaultIfEmpty()
            join charity in _queryDbContext.Charities
                on campaign.CharityId equals charity.Id
            join logoFile in _queryDbContext.StoredFiles
                on charity.LogoId equals logoFile.Id into logoFileGroup
            from logoFile in logoFileGroup.DefaultIfEmpty()
            select new CampaignsByCharityIdResponseDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                TotalAmount = campaign.TotalAmount,
                ChargedAmount = campaign.ChargedAmount,
                CharityName = charity.Name,
                ChargedAmountProgressPercentage = (campaign.TotalAmount > 0)
                    ? (campaign.ChargedAmount / campaign.TotalAmount * 100)
                    : 0,
                CampaignStatus = campaign.CampaignStatus,
                BannerUriAddress = bannerFile != null
                    ? $"{bannerFile.FilePath.Replace("\\", "/")}"
                    : $"/uploads/default-campaign.png",
                CharityLogoUriAddress = logoFile != null
                    ? $"{logoFile.FilePath.Replace("\\", "/")}"
                    : $"/uploads/default-charity.png"
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

        var campaignsQuery = from campaign in _queryDbContext.Campaigns
                .Include(c => c.Charity)
            where campaign.EndDate >= DateTime.UtcNow
            orderby campaign.StartDate
            join bannerFile in _queryDbContext.StoredFiles
                on campaign.BannerId equals bannerFile.Id into bannerFileGroup
            from bannerFile in bannerFileGroup.DefaultIfEmpty()
            join logoFile in _queryDbContext.StoredFiles
                on campaign.Charity.LogoId equals logoFile.Id into logoFileGroup
            from logoFile in logoFileGroup.DefaultIfEmpty()
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
                    ? bannerFile.FilePath.Replace("\\", "/")
                    : "/uploads/default-campaign.png",
                CharityLogoUriAddress = logoFile != null
                    ? logoFile.FilePath.Replace("\\", "/")
                    : "/uploads/default-charity.png"
            };

        var campaigns = await campaignsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync();

        return new PagedData<AllCampaignResponseDto>(campaigns, totalCount, query.PageSize, query.Page);
    }


    public async Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query)
    {
        var campaign = await _queryDbContext.Campaigns
                           .Include(c => c.Charity)
                           .FirstOrDefaultAsync(c => c.Id == query.Id)
                       ?? throw new KeyNotFoundException($"Campaign with ID {query.Id} not found.");

        var contributorCount = await _queryDbContext.Transactions
            .Where(t => t.CampaignId == campaign.Id)
            .Select(t => t.UserId)
            .Distinct()
            .CountAsync();

        var bannerFile = await _queryDbContext.StoredFiles
            .Where(f => f.Id == campaign.BannerId)
            .FirstOrDefaultAsync();

        var charityLogoFile = await _queryDbContext.StoredFiles
            .Where(f => f.Id == campaign.Charity.LogoId)
            .FirstOrDefaultAsync();

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
                : 0,
            ChargedAmount = campaign.ChargedAmount,
            TotalAmount = campaign.TotalAmount,
            CharityName = campaign.Charity.Name,
            CharityId = campaign.CharityId,
            CampaignStatus = campaign.CampaignStatus,
            BannerUriAddress = bannerFile != null
                ? bannerFile.FilePath.Replace("\\", "/")
                : "/uploads/default-campaign.png",
            CharityLogoUriAddress = charityLogoFile != null
                ? charityLogoFile.FilePath.Replace("\\", "/")
                : "/uploads/default-charity.png"
        };

        return campaignDto;
    }


    private static int CalculateRemainingDays(DateTime endDate)
    {
        var now = DateTime.UtcNow;
        var remainingDays = (endDate - now).TotalDays;

        return remainingDays > 0 ? (int)Math.Ceiling(remainingDays) : 0;
    }
}