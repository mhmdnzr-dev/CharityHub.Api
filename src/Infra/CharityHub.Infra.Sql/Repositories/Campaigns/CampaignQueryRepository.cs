namespace CharityHub.Infra.Sql.Repositories.Campaigns;

using CharityHub.Core.Contract.Primitives.Models;
using CharityHub.Core.Domain.Entities;
using CharityHub.Infra.Sql.Data.DbContexts;

using Core.Contract.Campaigns.Queries;
using Core.Contract.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Campaigns.Queries.GetCampaignById;
using Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public class CampaignQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<CampaignQueryRepository> logger)
    : QueryRepository<Campaign>(queryDbContext), ICampaignQueryRepository
{
    public async Task<PagedData<CampaignsByCharityIdResponseDto>> GetCampaignsByCharityId(
        GetCampaignsByCharityIdQuery query)
    {
        var totalCount = await _queryDbContext.Campaigns
            .Where(c => c.CharityId == query.Id)
            .CountAsync();

        var campaigns = await _queryDbContext.Campaigns
            .Where(c => c.CharityId == query.Id)
            .OrderBy(c => c.StartDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(campaign => new CampaignsByCharityIdResponseDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                TotalAmount = campaign.TotalAmount,
                ChargedAmount = campaign.ChargedAmount,
                CharityName = campaign.Charity.Name,
                ChargedAmountProgressPercentage = (campaign.ChargedAmount / campaign.TotalAmount * 100),
                CampaignStatus = campaign.CampaignStatus,
            })
            .ToArrayAsync();

        return new PagedData<CampaignsByCharityIdResponseDto>(campaigns, totalCount, query.PageSize, query.Page);
    }

    public async Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query)
    {
        var totalCount = await _queryDbContext.Campaigns.CountAsync();


        var campaigns = await _queryDbContext.Campaigns
            .Include(c => c.Charity)
            .OrderBy(c => c.StartDate) // Ensure ordering for consistent pagination
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(campaign => new AllCampaignResponseDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                CharityName = campaign.Charity.Name,
                RemainingDayCount = CalculateRemainingDays(campaign.EndDate),
                ContributionAmount = campaign.ChargedAmount,
                TotalAmount = campaign.TotalAmount,
                ProgressPercentage = (campaign.ChargedAmount / campaign.TotalAmount) * 100,
                CampaignStatus = campaign.CampaignStatus
            })
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
            .Select(t => t.UserId) // Select only UserIds
            .Distinct() // Get unique contributors
            .CountAsync(); // Count them

        var campaignDto = new CampaignByIdResponseDto
        {
            Id = campaign.Id,
            Title = campaign.Title,
            Description = campaign.Description,
            ContributionAmount = contributorCount,
            RemainingDayCount = CalculateRemainingDays(campaign.EndDate),
            StartDateTime = campaign.StartDate,
            EndDateTime = campaign.EndDate,
            ChargedAmountProgressPercentage = campaign.ChargedAmount / campaign.TotalAmount * 100,
            ChargedAmount = campaign.ChargedAmount,
            TotalAmount = campaign.TotalAmount,
            CharityName = campaign.Charity.Name,
            CharityId = campaign.CharityId,
            CampaignStatus = campaign.CampaignStatus
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