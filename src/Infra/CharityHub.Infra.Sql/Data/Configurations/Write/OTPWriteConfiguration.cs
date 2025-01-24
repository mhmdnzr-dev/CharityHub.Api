namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Domain.Entities.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


internal sealed class OTPWriteConfiguration : BaseEntityConfiguration<OTP>, IEntityTypeConfiguration<OTP>
{
    public void Configure(EntityTypeBuilder<OTP> entity)
    {
        base.Configure(entity);

        entity.Property(o => o.OtpCode)
            .IsRequired()
            .HasMaxLength(10);

        entity.Property(o => o.Status)
            .IsRequired();

        entity.Property(o => o.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        entity.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
