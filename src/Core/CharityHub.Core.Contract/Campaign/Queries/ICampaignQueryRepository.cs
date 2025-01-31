namespace CharityHub.Core.Contract.Campaign.Queries;

using Charity.Queries.GetCharityById;

using Domain.Entities;

using GetAllCampaigns;

using GetCampaignById;

using Primitives.Models;
using Primitives.Repositories;

public interface ICampaignQueryRepository : IQueryRepository<Campaign>
{
    Task<PagedData<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query);
    Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query);
}