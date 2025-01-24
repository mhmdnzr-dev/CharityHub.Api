namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CharityWriteConfiguration : BaseEntityConfiguration<Charity>, IEntityTypeConfiguration<Charity>
{
    public void Configure(EntityTypeBuilder<Charity> entity)
    {
        base.Configure(entity);

        entity.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(c => c.Description)
            .IsRequired(false);

        entity.Property(c => c.Website)
            .IsRequired(false)
            .HasMaxLength(255);

        entity.Property(c => c.Address)
            .IsRequired(false);

        entity.Property(c => c.Telephone)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        entity.Property(c => c.ManagerName)
            .IsRequired(false)
            .HasMaxLength(50);

        entity.Property(c => c.ContactName)
            .IsRequired(false)
            .HasMaxLength(50);

        entity.Property(c => c.ContactPhone)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        entity.HasOne(aut => aut.Social)
            .WithMany()
            .HasForeignKey(aut => aut.SocialId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationship with ApplicationUser (CreatedByUserId)
        entity.HasOne(c => c.ApplicationUser)  // Navigation property
            .WithMany()  // Assuming one ApplicationUser can create many Charities (one-to-many)
            .HasForeignKey(c => c.CreatedByUserId)  // Foreign key property (CreatedByUserId)
            .IsRequired()  // This relationship is required (not null)
            .OnDelete(DeleteBehavior.Restrict);  // Set delete behavior (optional)

        // Configure relationship with Campaigns
        entity.HasMany(c => c.Campaigns)
            .WithOne(ca => ca.Charity)
            .HasForeignKey(ca => ca.CharityId)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: Set delete behavior for campaigns

        // Configure CreatedAt property with default value
        entity.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
