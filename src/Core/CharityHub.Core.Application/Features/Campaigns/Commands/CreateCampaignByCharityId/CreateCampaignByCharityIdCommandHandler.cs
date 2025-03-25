namespace CharityHub.Core.Application.Features.Campaigns.Commands.CreateCampaignByCharityId;

using Contract.Features.Campaigns.Commands.CreateCampaignByCharityId;
using Contract.Features.Charity.Commands;
using Contract.Features.Charity.Queries;
using Contract.Primitives.Repositories;

using Domain.Entities;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Primitives;

public class CreateCampaignByCharityIdCommandHandler : CommandHandlerBase<CreateCampaignByCharityIdCommand>
{
    private readonly ICharityQueryRepository _charityQueryRepository;
    private readonly ICharityCommandRepository _charityCommandRepository;
    private readonly ILogger<CreateCampaignByCharityIdCommandHandler> _logger;

    public CreateCampaignByCharityIdCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor,
        ICharityQueryRepository charityQueryRepository, ICharityCommandRepository charityCommandRepository, ILogger<CreateCampaignByCharityIdCommandHandler> logger) : base(tokenService, httpContextAccessor)
    {
        _charityQueryRepository = charityQueryRepository;
        _charityCommandRepository = charityCommandRepository;
        _logger = logger;
    }

    public override async Task<int> Handle(CreateCampaignByCharityIdCommand command, CancellationToken cancellationToken)
    {
        var charity = await _charityQueryRepository.GetByIdAsync(command.CharityId);

        var campaign = Campaign.Create(
            command.Title,
            command.Description,
            command.StartDate,
            command.EndDate,
            command.TotalAmount,
            command.CityId,
            command.CharityId
        );

        charity.SetCampaign(campaign);

        await _charityCommandRepository.UpdateAsync(charity);
        
        _logger.LogInformation("Campaign created successfully for CharityId: {CharityId} with CampaignId: {CampaignId}", command.CharityId, campaign.Id);

        return campaign.Id;
    }
}