namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public sealed class ApplicationUserTerm : BaseEntity
{
    public int UserId { get; private set; }
    public ApplicationUser ApplicationUser { get; private set; }

    public int TermId { get; private set; }
    public Term Term { get; private set; }

    public int SocialId { get; private set; }
    public Social Social { get; private set; }

    public DateTime AcceptedAt { get; private set; } = DateTime.UtcNow;
}
