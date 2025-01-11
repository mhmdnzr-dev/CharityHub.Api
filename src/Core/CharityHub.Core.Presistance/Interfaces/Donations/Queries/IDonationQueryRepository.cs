namespace CharityHub.Core.Presistance.Interfaces.Donations.Queries;

using CharityHub.Core.Contract.Donations.DTOs.Queries.GetAllDonations;
using CharityHub.Core.Contract.Primitives;
using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Presistance.Interfaces.Base;

public interface IDonationQueryRepository : IQueryRepository<Donation>
{
    Task<PagedData<GetAllDonationsResponseDto>> GetPagedDonationsAsync(GetAllDonationsQuery query);
}
