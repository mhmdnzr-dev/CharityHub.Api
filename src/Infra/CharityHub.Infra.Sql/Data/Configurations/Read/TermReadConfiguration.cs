namespace CharityHub.Infra.Sql.Data.Configurations.Read;

using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class TermReadConfiguration : BaseEntityConfiguration<Term>, IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> entity)
    {
        base.Configure(entity);

        entity.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500);
    }
}
