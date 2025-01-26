

namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public sealed class Charity : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Website { get; private set; }

    public int CreatedByUserId { get; private set; }
    public ApplicationUser ApplicationUser { get; private set; }

    public string Address { get; private set; }

    public int? CityId { get; private set; }
    public string Telephone { get; private set; }
    public string ManagerName { get; private set; }

    public int LogoId { get; private set; }
    public int BannerId { get; private set; }

    public int SocialId { get; private set; }
    public Social Social { get; private set; }

    public string ContactName { get; private set; }
    public string ContactPhone { get; private set; }
    public ICollection<Campaign> Campaigns { get; private set; } = new HashSet<Campaign>();
    public string PhotoUriAddress { get; set; }
}