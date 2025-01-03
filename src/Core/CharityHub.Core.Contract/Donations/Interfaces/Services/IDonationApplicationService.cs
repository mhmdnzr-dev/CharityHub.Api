using CharityHub.Core.Contract.Donations.DTOs.Commands;
using CharityHub.Core.Contract.Donations.DTOs.Queries.GetAllDonations;
using CharityHub.Core.Contract.Donations.DTOs.Queries.GetDonationById;

namespace CharityHub.Core.Contract.Donations.Interfaces.Services;

public interface IDonationApplicationService
{
    Task<PagedData<GetAllDonationsResponseDto>> GetPagedDonationsAsync(GetAllDonationsQuery query);

    Task<GetDonationByIdResponseDto> GetDonationByIdAsync(GetDonationByIdQuery query);

    Task AddDonationAsync(CreateDonationCommand donation);

    Task UpdateDonationAsync(UpdateDonationCommand donation);

    Task DeleteDonationAsync(DeleteDonationCommand donationId);
}
