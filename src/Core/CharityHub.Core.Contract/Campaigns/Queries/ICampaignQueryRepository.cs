namespace CharityHub.Core.Contract.Campaigns.Queries;

using CharityHub.Core.Contract.Campaigns.Queries.GetAllCampaigns;
using CharityHub.Core.Contract.Campaigns.Queries.GetCampaignById;
using CharityHub.Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;
using CharityHub.Core.Contract.Primitives.Models;
using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.Entities;

public interface ICampaignQueryRepository : IQueryRepository<Campaign>
{
    Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query);
    Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query);
    Task<PagedData<CampaignsByCharityIdResponseDto>> GetCampaignsByCharityId(GetCampaignsByCharityIdQuery query);
}