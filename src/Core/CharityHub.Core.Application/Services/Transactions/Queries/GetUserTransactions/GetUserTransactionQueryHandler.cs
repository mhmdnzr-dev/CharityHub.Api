namespace CharityHub.Core.Application.Services.Transactions.Queries.GetUserTransactions;

using Contract.Transactions.Queries;
using Contract.Transactions.Queries.GetUserTransactions;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class
    GetUserTransactionQueryHandler : QueryHandlerBase<GetUserTransactionQuery, IEnumerable<UserTransactionsResponseDto>>
{
    private readonly ITransactionQueryRepository _transactionQueryRepository;

    public GetUserTransactionQueryHandler(IMemoryCache cache, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, ITransactionQueryRepository transactionQueryRepository) : base(cache, tokenService, httpContextAccessor)
    {
        _transactionQueryRepository = transactionQueryRepository;
    }
    public override async Task<IEnumerable<UserTransactionsResponseDto>> Handle(GetUserTransactionQuery query,
        CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader();
        var userDetails = await TokenService.GetUserByTokenAsync(new GetUserByTokenRequest { Token = token });
        var transactions = await _transactionQueryRepository.GetTransactionsByUserId(query, userDetails.Id);
        return transactions;
    }
}