namespace CharityHub.Infra.Sql.Data.Configurations.Write;

using Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


internal sealed class MessageWriteConfiguration : BaseEntityConfiguration<Message>, IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> entity)
    {
        base.Configure(entity);
        
        
        entity.HasOne(m => m.User)
            .WithMany() 
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

    
        entity.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(1000); 

        entity.Property(m => m.IsSeen)
            .IsRequired();

        entity.Property(m => m.SeenDateTime)
            .HasColumnType("datetime2")
            .IsRequired(false);
    }
}
