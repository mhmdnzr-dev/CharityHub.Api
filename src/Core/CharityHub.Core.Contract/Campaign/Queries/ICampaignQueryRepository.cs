namespace CharityHub.Core.Contract.Campaign.Queries;

using Charity.Queries.GetCharityById;

using Domain.Entities;

using GetAllCamaigns;

using GetCampaignById;

using Primitives.Repositories;

public interface ICampaignQueryRepository : IQueryRepository<Campaign>
{
    Task<IEnumerable<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query);
    Task<CampaignByIdResponseDto> GetDetailedById(GetCampaignByIdQuery query);
}