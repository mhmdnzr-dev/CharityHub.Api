namespace CharityHub.Infra.Sql.Repositories.Campaign;

using Charities;

using Core.Contract.Campaign.Queries;
using Core.Contract.Campaign.Queries.GetAllCampaigns;
using Core.Contract.Campaign.Queries.GetCampaignById;
using Core.Contract.Charity.Queries;
using Core.Contract.Primitives.Models;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Premitives;

public class CampaignQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<CampaignQueryRepository> logger)
    : QueryRepository<Campaign>(queryDbContext), ICampaignQueryRepository
{
    public async Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query)
    {
        var totalCount = await _queryDbContext.Campaigns.CountAsync();

        var campaigns = await _queryDbContext.Campaigns
            .Include(c => c.Charity)
            .OrderBy(c => c.StartDate) // Ensure ordering for consistent pagination
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new AllCampaignResponseDto
            {
                Name = c.Title,
                CharityName = c.Charity.Name,
                ContributionAmount = c.ChargedAmount,
                StartDateTime = c.StartDate
            })
            .ToArrayAsync();

        return new PagedData<AllCampaignResponseDto>(campaigns, totalCount, query.PageSize, query.Page);
    }


    public async Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query)
    {
        var campaign = await _queryDbContext.Campaigns
            .Include(c => c.Charity)
            .FirstOrDefaultAsync(c => c.Id == query.Id);

        if (campaign == null)
        {
            return new CampaignByIdResponseDto(); 
        }

        var campaignDto = new CampaignByIdResponseDto
        {
            Title = campaign.Title,
            DonorCount = 0,
            RemainingDayCount = 0, 
            StartDateTime = campaign.StartDate,
            ChargedAmountProgressPercentage = campaign.ChargedAmount / campaign.TotalAmount * 100, 
            TotalAmount = campaign.TotalAmount
        };

        return campaignDto;
    }
}