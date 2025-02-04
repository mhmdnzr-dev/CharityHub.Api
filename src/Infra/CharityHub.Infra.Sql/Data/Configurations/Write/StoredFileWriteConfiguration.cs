namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class StoredFileWriteConfiguration : BaseEntityConfiguration<StoredFile>,
    IEntityTypeConfiguration<StoredFile>
{
    public void Configure(EntityTypeBuilder<StoredFile> entity)
    {
        base.Configure(entity);


        entity.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(f => f.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(f => f.FileType)
            .IsRequired()
            .HasMaxLength(100);
    }
}