namespace CharityHub.Core.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;

using Identity;

using ValueObjects;

public class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }

    public int UserId { get; private set; }

    public int CampaignId { get; private set; }

    [ForeignKey("CampaignId")]
    public virtual Campaign Campaign { get; set; }


    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
}