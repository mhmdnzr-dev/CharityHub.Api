namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class DonationWriteConfiguration : BaseEntityConfiguration<Donation>, IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> entity)
    {
        base.Configure(entity);

        entity.Property(d => d.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        entity.Property(d => d.DonatedAt)
            .IsRequired();

        entity.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(d => d.Campaign)
            .WithMany(c => c.Donations)
            .HasForeignKey(d => d.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
