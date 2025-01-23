namespace CharityHub.Infra.Sql.Data.Configurations.Write;
using CharityHub.Core.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CharityCategoryConfiguration : IEntityTypeConfiguration<CharityCategory>
{
    public void Configure(EntityTypeBuilder<CharityCategory> builder)
    {
        builder.HasNoKey();

        builder.HasOne(cc => cc.Charity)
            .WithMany()
            .HasForeignKey(cc => cc.CharityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Category)
            .WithMany()
            .HasForeignKey(cc => cc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
