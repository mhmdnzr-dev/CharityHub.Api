namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CampaignCategoryWriteConfiguration : IEntityTypeConfiguration<CampaignCategory>
{
    public void Configure(EntityTypeBuilder<CampaignCategory> entity)
    {
        entity.HasNoKey();

        entity.HasOne(cc => cc.Campaign)
            .WithMany()
            .HasForeignKey(cc => cc.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(cc => cc.Category)
            .WithMany()
            .HasForeignKey(cc => cc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
