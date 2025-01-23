

namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public class Charity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Website { get; set; }

    public int CreatedByUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public string Address { get; set; }

    public int? CityId { get; set; }
    public string Telephone { get; set; }
    public string ManagerName { get; set; }

    public int? SocialId { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public ICollection<Campaign> Campaigns { get; set; } = new HashSet<Campaign>();
}