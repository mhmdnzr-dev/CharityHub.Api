namespace CharityHub.Core.Contract.Charity.Commands;

using Domain.Entities;

using Primitives.Repositories;

public interface ICharityCommandRepository : ICommandRepository<Charity>
{
}