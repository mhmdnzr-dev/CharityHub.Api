namespace CharityHub.Core.Domain.Entities.Users;
using System;

using CharityHub.Core.Domain.ValueObjects;


public class UserDonation : Aggregate<User>
{
    public int CharityId { get; set; }
    public int CampaignId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DonatedAt { get; set; }
}