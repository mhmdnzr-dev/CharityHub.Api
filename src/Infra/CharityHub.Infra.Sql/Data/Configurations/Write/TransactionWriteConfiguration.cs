namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class TransactionWriteConfiguration : BaseEntityConfiguration<Transaction>, IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> entity)
    {
        base.Configure(entity);
        entity.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        entity.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(t => t.Campaign)
            .WithMany()
            .HasForeignKey(t => t.CampaignId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional: Configure any other properties, indexes, etc.
        // Example: entity.HasIndex(t => t.UserId).IsUnique();  // If UserId should be unique
    }
}