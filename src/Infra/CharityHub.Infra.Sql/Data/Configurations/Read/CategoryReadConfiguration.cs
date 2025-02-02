namespace CharityHub.Infra.Sql.Data.Configurations.Read;

using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CategoryReadConfiguration : BaseEntityConfiguration<Category>, IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        base.Configure(entity);

        entity.Property(c => c.Name)
            .IsRequired();
    }
}