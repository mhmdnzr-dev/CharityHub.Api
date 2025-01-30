namespace CharityHub.Infra.Sql.Data.SeedData;

using System.Threading;
using System.Threading.Tasks;

public interface ISeeder<TContext>
{
    Task SeedAsync(TContext context, CancellationToken cancellationToken = default);
}
