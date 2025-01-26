namespace CharityHub.Core.Contract.Campaign.Queries;

using Domain.Entities;

using GetAllCamaigns;

using Primitives.Repositories;

public interface ICampaignQueryRepository:IQueryRepository<Campaign>
{
    Task<IEnumerable<AllCampaignResponseDto>> GetAllCampaignsAsync(GetAllCampaignQuery query);
}