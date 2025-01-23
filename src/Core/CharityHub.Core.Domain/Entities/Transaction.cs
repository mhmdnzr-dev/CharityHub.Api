
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Core.Domain.ValueObjects;


namespace CharityHub.Core.Domain.Entities;



public class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }
    public int UserId { get; private set; }
    public ApplicationUser User { get; private set; }
    public int CampaignId { get; private set; }
}