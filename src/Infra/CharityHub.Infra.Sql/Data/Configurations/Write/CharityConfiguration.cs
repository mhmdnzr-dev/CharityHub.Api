namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CharityConfiguration : IEntityTypeConfiguration<Charity>
{
    public void Configure(EntityTypeBuilder<Charity> builder)
    {
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

        builder.Property(c => c.ContactPhoneNumber)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        builder.HasOne(c => c.CreatedByNavigation)
            .WithMany()
            .HasForeignKey(c => c.CreatedByUserId);

        builder.HasMany(c => c.Campaigns)
            .WithOne(ca => ca.Charity)
            .HasForeignKey(ca => ca.CharityId);
    }
}
