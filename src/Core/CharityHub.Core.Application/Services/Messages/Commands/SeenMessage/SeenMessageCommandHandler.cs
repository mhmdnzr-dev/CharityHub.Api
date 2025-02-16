namespace CharityHub.Core.Application.Services.Messages.Commands.SeenMessage;

using Contract.Messages.Commands;
using Contract.Messages.Commands.SeenMessage;
using Contract.Messages.Queries;
using Contract.Primitives.Repositories;

using Infra.Identity.Interfaces;

using Primitives;

public class SeenMessageCommandHandler : CommandHandlerBase<SeenMessageCommand>
{
    private readonly IMessageCommandRepository _messageCommandRepository;
    private readonly IMessageQueryRepository _messageQueryRepository;


    public SeenMessageCommandHandler(ITokenService tokenService, IMessageCommandRepository messageCommandRepository,
        IMessageQueryRepository messageQueryRepository) : base(tokenService)
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