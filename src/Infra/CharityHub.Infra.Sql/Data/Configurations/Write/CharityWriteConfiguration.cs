namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CharityWriteConfiguration : IEntityTypeConfiguration<Charity>
{
    public void Configure(EntityTypeBuilder<Charity> builder)
    {
        // Configure basic properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Description)
            .IsRequired(false);

        builder.Property(c => c.Website)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(c => c.Address)
            .IsRequired(false);

        builder.Property(c => c.Telephone)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        builder.Property(c => c.ManagerName)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(c => c.ContactName)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(c => c.ContactPhone)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        // Configure relationship with ApplicationUser (CreatedByUserId)
        builder.HasOne(c => c.ApplicationUser)  // Navigation property
            .WithMany()  // Assuming one ApplicationUser can create many Charities (one-to-many)
            .HasForeignKey(c => c.CreatedByUserId)  // Foreign key property (CreatedByUserId)
            .IsRequired()  // This relationship is required (not null)
            .OnDelete(DeleteBehavior.Restrict);  // Set delete behavior (optional)

        // Configure relationship with Campaigns
        builder.HasMany(c => c.Campaigns)
            .WithOne(ca => ca.Charity)
            .HasForeignKey(ca => ca.CharityId)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: Set delete behavior for campaigns

        // Configure CreatedAt property with default value
        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
