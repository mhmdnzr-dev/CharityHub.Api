using System.Reflection;

using CharityHub.Core.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Data.DbContexts;


public class CharityHubQueryDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    #region DbSets

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

        modelBuilder.Ignore<ApplicationUser>();
        modelBuilder.Ignore<ApplicationRole>();
    }
}
