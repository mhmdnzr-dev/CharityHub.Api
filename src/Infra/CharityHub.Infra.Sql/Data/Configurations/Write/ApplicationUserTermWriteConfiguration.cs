namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using CharityHub.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ApplicationUserTermWriteConfiguration : BaseEntityConfiguration<ApplicationUserTerm>, IEntityTypeConfiguration<ApplicationUserTerm>
{
    public void Configure(EntityTypeBuilder<ApplicationUserTerm> entity)
    {
        base.Configure(entity);

        entity.HasOne(aut => aut.ApplicationUser)
            .WithMany()
            .HasForeignKey(aut => aut.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(aut => aut.Term)
            .WithMany()
            .HasForeignKey(aut => aut.TermId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);



        entity.Property(aut => aut.AcceptedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
