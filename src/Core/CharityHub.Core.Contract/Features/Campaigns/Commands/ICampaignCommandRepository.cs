namespace CharityHub.Core.Contract.Features.Campaigns.Commands;

using Domain.Entities;

using Primitives.Repositories;

public interface ICampaignCommandRepository : ICommandRepository<Campaign>
{
}