namespace CharityHub.Core.Application.Services.Messages.Queries.GetMessagesByUserId;

using Contract.Messages.Queries;
using Contract.Messages.Queries.GetMessageByUserIdQuery;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetMessageByUserIdQueryHandler : QueryHandlerBase<GetMessageByUserIdQuery, IEnumerable<MessageByUserIdDto>>
{
    private readonly IMessageQueryRepository _messageQueryRepository;


    public GetMessageByUserIdQueryHandler(IMemoryCache cache, ITokenService tokenService,
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