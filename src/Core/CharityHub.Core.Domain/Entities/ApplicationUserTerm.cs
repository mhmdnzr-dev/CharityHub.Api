namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public sealed class ApplicationUserTerm : BaseEntity
{
    public int UserId { get; private set; }
    public ApplicationUser ApplicationUser { get; private set; }

    public int TermId { get; private set; }
    public Term Term { get; private set; }

    public DateTime AcceptedAt { get; private set; } = DateTime.UtcNow;

    // Private constructor to enforce controlled instantiation via factory method
    private ApplicationUserTerm(int userId, int termId, DateTime? acceptedAt = null)
    {
        UserId = userId;
        TermId = termId;
        AcceptedAt = acceptedAt ?? DateTime.UtcNow;
    }

    // Factory method to add a new ApplicationUserTerm
    public static ApplicationUserTerm Add(int userId, int termId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Invalid UserId.");
        }

        if (termId <= 0)
        {
            throw new ArgumentException("Invalid TermId.");
        }


        return new ApplicationUserTerm(userId, termId);
    }
}
