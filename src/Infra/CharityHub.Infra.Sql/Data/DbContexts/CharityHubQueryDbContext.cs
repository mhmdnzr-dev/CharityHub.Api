using System.Reflection;

using CharityHub.Core.Domain.Entities;
using CharityHub.Core.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Data.DbContexts;

public sealed class CharityHubQueryDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    #region DbSets

    public DbSet<OTP> OTPs { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Charity> Charities { get; set; }
    public DbSet<StoredFile> StoredFiles { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Social> Socials { get; set; }
    public DbSet<Message> Messages { get; set; }

    #endregion

    #region Ctor

    // Parameterless constructor for design-time
    public CharityHubQueryDbContext() : base() { }

    // Constructor with DbContextOptions for runtime
    public CharityHubQueryDbContext(DbContextOptions<CharityHubQueryDbContext> options) : base(options)
    {
    }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
            type => type.Name.EndsWith("ReadConfiguration"));
    }
}