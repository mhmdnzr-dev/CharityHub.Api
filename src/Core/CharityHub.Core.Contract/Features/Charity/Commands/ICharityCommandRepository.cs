namespace CharityHub.Core.Contract.Features.Charity.Commands;

using Domain.Entities;

using Primitives.Repositories;

public interface ICharityCommandRepository : ICommandRepository<Charity>
{
}