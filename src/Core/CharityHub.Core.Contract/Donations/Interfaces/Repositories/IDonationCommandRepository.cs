using CharityHub.Core.Contract.Donations.DTOs.Commands;

namespace CharityHub.Core.Contract.Donations.Interfaces.Repositories;

public interface IDonationCommandRepository
{
    Task AddDonationAsync(CreateDonationCommand donation);

    Task UpdateDonationAsync(UpdateDonationCommand donation);

    Task DeleteDonationAsync(DeleteDonationCommand donationId);
}
