namespace CharityHub.Core.Application.Features.Messages.Commands.SeenMessage;

using Contract.Features.Messages.Commands;
using Contract.Features.Messages.Commands.SeenMessage;
using Contract.Features.Messages.Queries;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;

using Primitives;

public class SeenMessageCommandHandler : CommandHandlerBase<SeenMessageCommand>
{
    private readonly IMessageCommandRepository _messageCommandRepository;
    private readonly IMessageQueryRepository _messageQueryRepository;


    public SeenMessageCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor,
        IMessageCommandRepository messageCommandRepository, IMessageQueryRepository messageQueryRepository) : base(
        tokenService, httpContextAccessor)
    {
        _messageCommandRepository = messageCommandRepository;
        _messageQueryRepository = messageQueryRepository;
    }

    public override async Task<int> Handle(SeenMessageCommand command, CancellationToken cancellationToken)
    {
        var messageModel = await _messageQueryRepository.GetByIdAsync(command.Id);
        messageModel.MarkAsSeen();

        try
        {
            await _messageCommandRepository.UpdateAsync(messageModel);

            return command.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}