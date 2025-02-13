using System.Reflection;

using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Data.DbContexts;

using Core.Domain.Entities.Common;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

public sealed class CharityHubCommandDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    #region DbSets

    public DbSet<ApplicationUserTerm> ApplicationUserTerms { get; set; }
   
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<OTP> OTPs { get; set; }
    public DbSet<Social> Socials { get; set; }
    
    public DbSet<StoredFile> StoredFiles { get; set; }
    public DbSet<Term> Terms { get; set; }
    
    public DbSet<CampaignCategory> CampaignCategories { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Charity> Charities { get; set; }

    public DbSet<CharityCategory> CharityCategories { get; set; }

    public DbSet<Donation> Donations { get; set; }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), type => type.Name.EndsWith("WriteConfiguration"));
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
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified))
            .ToList(); // Convert to list to avoid collection modified exception during iteration

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            // Skip entities that are mapper tables (like CampaignCategory) that don't have CreatedAt/ModifiedAt
            if (entityEntry.Entity is CampaignCategory) continue;

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
