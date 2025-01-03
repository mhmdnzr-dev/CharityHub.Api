using System.Reflection;

using CharityHub.Core.Domain.Entities.Donations;
using CharityHub.Core.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Data.DbContexts;


public class CharityHubQueryDbContext : IdentityDbContext<User, UserRole, string>
{
    #region DbSets
    public DbSet<Donation> Donations { get; set; }
    #endregion

    public CharityHubQueryDbContext(DbContextOptions<CharityHubQueryDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
