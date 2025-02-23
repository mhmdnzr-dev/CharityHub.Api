namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CampaignWriteConfiguration : BaseEntityConfiguration<Campaign>, IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> entity)
    {
        base.Configure(entity);

        entity.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(1000);

        entity.Property(c => c.StartDate)
            .IsRequired();

        entity.Property(c => c.EndDate)
            .IsRequired();

        entity.Property(c => c.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        entity.Property(c => c.ChargedAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);




        entity.HasOne(c => c.Charity)
            .WithMany(ch => ch.Campaigns)
            .HasForeignKey(c => c.CharityId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        entity.HasMany(c => c.Donations)
            .WithOne(d => d.Campaign)
            .HasForeignKey(d => d.CampaignId);
        
        entity.HasOne(c => c.Banner)
            .WithMany()
            .HasForeignKey(c => c.BannerId)
            .OnDelete(DeleteBehavior.NoAction);     
    }
}
