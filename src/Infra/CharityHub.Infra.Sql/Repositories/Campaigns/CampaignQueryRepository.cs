namespace CharityHub.Infra.Sql.Repositories.Campaigns;

using CharityHub.Core.Contract.Primitives.Models;
using CharityHub.Core.Domain.Entities;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Premitives;

using Core.Contract.Campaigns.Queries;
using Core.Contract.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Campaigns.Queries.GetCampaignById;
using Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            .Select(c => new CampaignsByCharityIdResponseDto
            {
                Id = c.Id,
                Name = c.Title,
                Description = c.Description,
                TotalAmount = c.TotalAmount,
                ChargedAmount = c.ChargedAmount,
                CharityName = c.Charity.Name,
                Percentage = (c.ChargedAmount / c.TotalAmount * 100)
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