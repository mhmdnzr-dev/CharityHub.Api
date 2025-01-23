using System.Reflection;

using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Core.Domain.ValueObjects;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Data.DbContexts;



public partial class CharityHubCommandDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    #region DbSets
    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<CampaignCategory> CampaignCategories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Charity> Charities { get; set; }

    public virtual DbSet<CharityCategory> CharityCategories { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
    #endregion


    #region Constructor
    private CharityHubCommandDbContext()
    {

    }
    public CharityHubCommandDbContext(DbContextOptions<CharityHubCommandDbContext> options) : base(options)
    {
    }
    #endregion




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        modelBuilder.Entity<Charity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Charity__8189E594F67961C8");

            entity.Property(e => e.ContactName).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ManagerName).IsFixedLength();

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Charities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Charity__Created__3B75D760");
        });

        modelBuilder.Entity<CharityCategory>(entity =>
        {
            entity.HasNoKey();
            entity.HasOne(d => d.Category).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharityCategory_Category");

            entity.HasOne(d => d.Charity).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharityCategory_Charity");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Donation__3214EC07A3F2D206");

            entity.Property(e => e.DonatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Donations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donation_Campaign");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();

        return base.SaveChangesAsync(cancellationToken);
    }
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entity.ModifiedAt = DateTime.UtcNow;
                entityEntry.Property(nameof(entity.CreatedAt)).IsModified = false;
            }
        }
    }
}
