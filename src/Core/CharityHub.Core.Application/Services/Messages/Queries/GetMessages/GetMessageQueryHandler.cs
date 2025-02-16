namespace CharityHub.Core.Application.Services.Messages.Queries.GetMessages;

using CharityHub.Core.Application.Primitives;
using CharityHub.Core.Contract.Messages.Queries;
using CharityHub.Core.Contract.Messages.Queries.GetMessageByUserIdQuery;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models.Token.Requests;

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