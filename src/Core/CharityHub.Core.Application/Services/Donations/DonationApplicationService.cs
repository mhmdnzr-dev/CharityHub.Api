using CharityHub.Core.Contract;
using CharityHub.Core.Contract.Donations.DTOs.Commands;
using CharityHub.Core.Contract.Donations.DTOs.Queries.GetAllDonations;
using CharityHub.Core.Contract.Donations.DTOs.Queries.GetDonationById;
using CharityHub.Core.Contract.Donations.Interfaces.Services;

namespace CharityHub.Core.Application.Services.Donations;


public class DonationApplicationService : IDonationApplicationService
{
    public Task AddDonationAsync(CreateDonationCommand donation)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDonationAsync(DeleteDonationCommand donationId)
    {
        throw new NotImplementedException();
    }

    public Task<GetDonationByIdResponseDto> GetDonationByIdAsync(GetDonationByIdQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<PagedData<GetAllDonationsResponseDto>> GetPagedDonationsAsync(GetAllDonationsQuery query)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDonationAsync(UpdateDonationCommand donation)
    {
        throw new NotImplementedException();
    }
}
