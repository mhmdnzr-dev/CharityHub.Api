namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CharityCategoryWriteConfiguration : IEntityTypeConfiguration<CharityCategory>
{
    public void Configure(EntityTypeBuilder<CharityCategory> entity)
    {
        entity.HasNoKey();

        entity.HasOne(cc => cc.Charity)
            .WithMany()
            .HasForeignKey(cc => cc.CharityId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(cc => cc.Category)
            .WithMany()
            .HasForeignKey(cc => cc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
