using CharityHub.Core.Domain.Entities.Donations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharityHub.Infra.Sql.Data.Configurations;



public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        // Configure the Donation entity
        builder.ToTable("Donations");  // Define table name if necessary
        builder.HasKey(d => d.Id);     // Set the primary key

        builder.Property(d => d.Amount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");  // Column type configuration

        builder.Property(d => d.Date)
               .IsRequired()
               .HasColumnType("datetime");  // Column type configuration
    }
}
