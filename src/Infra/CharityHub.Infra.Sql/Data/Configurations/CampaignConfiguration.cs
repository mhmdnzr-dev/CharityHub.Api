using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharityHub.Infra.Sql.Data.Configurations;



public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        // Configure the Donation entity
        builder.ToTable("Campaigns");  // Define table name if necessary
        builder.HasKey(d => d.Id);     // Set the primary key


    }
}
