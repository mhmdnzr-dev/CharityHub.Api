namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using CharityHub.Core.Domain.ValueObjects;

using Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CampaignCategoryWriteConfiguration : IEntityTypeConfiguration<CampaignCategory>
{
    public void Configure(EntityTypeBuilder<CampaignCategory> entity)
    {
        // Define the composite primary key
        entity.HasKey(cc => new { cc.CampaignId, cc.CategoryId });

        // Define the foreign key relationship with Campaign
        entity.HasOne(cc => cc.Campaign)
            .WithMany(c => c.CampaignCategories)
            .HasForeignKey(cc => cc.CampaignId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

        // Define the foreign key relationship with Category
        entity.HasOne(cc => cc.Category)
            .WithMany(c => c.CampaignCategories)
            .HasForeignKey(cc => cc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths
    }
}
