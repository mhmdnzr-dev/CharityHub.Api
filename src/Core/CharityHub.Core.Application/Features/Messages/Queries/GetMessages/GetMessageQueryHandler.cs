namespace CharityHub.Core.Application.Features.Messages.Queries.GetMessages;

using Primitives;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;

using Contract.Features.Messages.Queries;
using Contract.Features.Messages.Queries.GetMessageByUserIdQuery;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

public class GetMessageQueryHandler : QueryHandlerBase<GetMessageByUserIdQuery, IEnumerable<MessageByUserIdDto>>
{
    private readonly IMessageQueryRepository _messageQueryRepository;


    public GetMessageQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, IMessageQueryRepository messageQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
    {
        _messageQueryRepository = messageQueryRepository;
    }

    public override async Task<IEnumerable<MessageByUserIdDto>> Handle(GetMessageByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader();
        var userDetails = await TokenService.GetUserByTokenAsync(new GetUserByTokenRequest { Token = token });
        var result = await _messageQueryRepository.GetAllByUserId(userDetails.Id);
        return result;
    }
}