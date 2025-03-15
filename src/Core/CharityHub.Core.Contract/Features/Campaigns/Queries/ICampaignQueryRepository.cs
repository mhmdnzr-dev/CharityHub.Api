namespace CharityHub.Core.Contract.Features.Campaigns.Queries;


using Primitives.Models;
using Primitives.Repositories;
using Domain.Entities;

using GetAllCampaigns;

using GetCampaignById;

using GetCampaignsByCharityId;

public interface ICampaignQueryRepository : IQueryRepository<Campaign>
{
    Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query);
    Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query);
    Task<PagedData<CampaignsByCharityIdResponseDto>> GetCampaignsByCharityId(GetCampaignsByCharityIdQuery query);
}