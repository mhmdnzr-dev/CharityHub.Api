using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.Entities;

namespace CharityHub.Core.Contract.Donations.Commands;


public interface IDonationCommandRepository : ICommandRepository<Donation>
{

}
