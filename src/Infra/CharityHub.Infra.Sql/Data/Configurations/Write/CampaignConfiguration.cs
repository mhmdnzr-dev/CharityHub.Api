namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(c => c.StartDate)
            .IsRequired(false);

        builder.Property(c => c.EndDate)
            .IsRequired(false);

        builder.Property(c => c.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(c => c.ChargedAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(c => c.Photo)
            .IsFixedLength()
            .IsRequired(false);



        builder.HasOne(c => c.Charity)
            .WithMany(ch => ch.Campaigns)
            .HasForeignKey(c => c.CharityId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasMany(c => c.Donations)
            .WithOne(d => d.Campaign)
            .HasForeignKey(d => d.CampaignId);
    }
}
