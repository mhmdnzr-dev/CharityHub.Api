namespace CharityHub.Infra.Sql.Data.DbContexts;

using System.Reflection;

using CharityHub.Core.Domain.Entities.Donations;
using CharityHub.Core.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CharityHubCommandDbContext : IdentityDbContext<User, UserRole, string>
{
    #region DbSets
    public DbSet<Donation> Donations { get; set; }
    #endregion
    public CharityHubCommandDbContext()
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
