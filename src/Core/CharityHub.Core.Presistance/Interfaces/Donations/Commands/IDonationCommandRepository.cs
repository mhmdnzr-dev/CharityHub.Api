using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Presistance.Interfaces.Base;

namespace CharityHub.Core.Presistance.Interfaces.Donations.Commands;


public interface IDonationCommandRepository : ICommandRepository<Donation>
{
   
}
