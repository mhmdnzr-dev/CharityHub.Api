namespace CharityHub.Infra.Sql.Data.Configurations;
using CharityHub.Core.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.ModifiedAt)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}