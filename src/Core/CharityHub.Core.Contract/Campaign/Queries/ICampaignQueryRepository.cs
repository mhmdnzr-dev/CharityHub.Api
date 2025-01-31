namespace CharityHub.Core.Contract.Campaign.Queries;

using Domain.Entities;

using GetAllCampaigns;

using GetCampaignById;

using GetCampaignsByCharityId;

using Primitives.Models;
using Primitives.Repositories;

public interface ICampaignQueryRepository : IQueryRepository<Campaign>
{
    Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query);
    Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query);
    Task<PagedData<CampaignsByCharityIdResponseDto>> GetCampaignsByCharityId(GetCampaignsByCharityIdQuery query);
}