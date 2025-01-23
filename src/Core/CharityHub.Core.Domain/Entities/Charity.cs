

namespace CharityHub.Core.Domain.Entities;

using System.ComponentModel.DataAnnotations;

using Identity;

using Microsoft.EntityFrameworkCore;

using ValueObjects;

public class Charity : BaseEntity
{

    [StringLength(255)]
    public string Name { get; private set; }

    public string Description { get; private set; }

    [StringLength(255)]
    public string Website { get; private set; }

    public int CreatedByUserId { get; private set; }
    public ApplicationUser CreatedByNavigation { get; private set; }

    public string Address { get; private set; }

    public int? CityId { get; private set; }

    [StringLength(15)]
    [Unicode(false)]
    public string Telephone { get; private set; }

    [StringLength(50)]
    public string ManagerName { get; private set; }

    public int? SocialId { get; private set; }

    [StringLength(50)]
    public string ContactName { get; private set; }

    [StringLength(15)]
    [Unicode(false)]
    public string ContactPhoneNumber { get; private set; }

    public ICollection<Campaign> Campaigns { get; private set; } = new HashSet<Campaign>();

}