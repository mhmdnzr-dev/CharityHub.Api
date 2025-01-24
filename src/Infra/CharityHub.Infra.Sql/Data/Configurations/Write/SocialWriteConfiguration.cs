namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class SocialWriteConfiguration : BaseEntityConfiguration<Social>, IEntityTypeConfiguration<Social>
{
    public void Configure(EntityTypeBuilder<Social> entity)
    {
        base.Configure(entity);

        entity.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(s => s.Abbreviation)
            .IsRequired()
            .HasMaxLength(10);
    }
}