namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CharityWriteConfiguration : BaseEntityConfiguration<Charity>, IEntityTypeConfiguration<Charity>
{
    public void Configure(EntityTypeBuilder<Charity> entity)
    {
        base.Configure(entity);

        entity.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(c => c.Description)
            .IsRequired(false);

        entity.Property(c => c.Website)
            .IsRequired(false)
            .HasMaxLength(255);

        entity.Property(c => c.Address)
            .IsRequired(false);

        entity.Property(c => c.Telephone)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        entity.Property(c => c.ManagerName)
            .IsRequired(false)
            .HasMaxLength(50);

        entity.Property(c => c.ContactName)
            .IsRequired(false)
            .HasMaxLength(50);

        entity.Property(c => c.ContactPhone)
            .IsRequired(false)
            .HasMaxLength(15)
            .IsUnicode(false);

        entity.HasOne(aut => aut.Social)
            .WithMany()
            .HasForeignKey(aut => aut.SocialId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        entity.HasOne(c => c.Logo)
            .WithMany()
            .HasForeignKey(c => c.LogoId)
            .OnDelete(DeleteBehavior.NoAction); // No automatic action on delete

        entity.HasOne(c => c.Banner)
            .WithMany()
            .HasForeignKey(c => c.BannerId)
            .OnDelete(DeleteBehavior.NoAction); // No automatic action on delete


        entity.HasOne(c => c.ApplicationUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedByUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        entity.HasMany(c => c.Campaigns)
            .WithOne(ca => ca.Charity)
            .HasForeignKey(ca => ca.CharityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}