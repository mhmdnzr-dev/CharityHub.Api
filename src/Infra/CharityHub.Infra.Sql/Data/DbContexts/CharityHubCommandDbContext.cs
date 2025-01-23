using System.Reflection;

using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Core.Domain.ValueObjects;
using CharityHub.Infra.Sql.Data.Configurations;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CharityHub.Infra.Sql.Data.DbContexts;



public partial class CharityHubCommandDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
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


    #region Ctor
    // Parameterless constructor for design-time
    public CharityHubCommandDbContext() : base() { }

    // Constructor with DbContextOptions for runtime
    public CharityHubCommandDbContext(DbContextOptions<CharityHubCommandDbContext> options) : base(options)
    {
    }
    #endregion


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
            type => type.Name.EndsWith("WriteConfiguration")
                   || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(BaseEntityConfiguration<>)));
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
