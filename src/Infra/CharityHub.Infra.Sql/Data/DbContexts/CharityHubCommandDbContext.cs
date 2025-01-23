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
    }


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
