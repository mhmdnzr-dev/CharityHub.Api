namespace CharityHub.Core.Contract.Donations.Interfaces.Repositories;
using CharityHub.Core.Contract.Donations.DTOs.Queries.GetAllDonations;
using CharityHub.Core.Contract.Donations.DTOs.Queries.GetDonationById;

public interface IDonationQueryRepository
{
    Task<PagedData<GetAllDonationsResponseDto>> GetPagedDonationsAsync(GetAllDonationsQuery query);

    Task<GetDonationByIdResponseDto> GetDonationByIdAsync(GetDonationByIdQuery query);
}
