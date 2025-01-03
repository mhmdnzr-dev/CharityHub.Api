namespace CharityHub.Core.Domain.Entities.Users;
using System;


public class UserDonation
{
    public int CharityId { get; set; }
    public int CampaignId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DonatedAt { get; set; }
}