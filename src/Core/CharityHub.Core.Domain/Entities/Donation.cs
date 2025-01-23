namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public class Donation : BaseEntity
{
    public int UserId { get; private set; }
    public ApplicationUser User { get; private set; }
    public int CampaignId { get; private set; }
    public Campaign Campaign { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DonatedAt { get; private set; }
}