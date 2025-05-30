namespace CharityHub.Core.Application.Features.Transactions.Queries.GetUserTransactions;

using Contract.Features.Transactions.Queries;
using Contract.Features.Transactions.Queries.GetUserTransactions;
using Contract.Primitives.Models;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class
    GetUserTransactionQueryHandler : QueryHandlerBase<GetUserTransactionQuery, PagedData<UserTransactionsResponseDto>>
{
    private readonly ITransactionQueryRepository _transactionQueryRepository;

    public GetUserTransactionQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, ITransactionQueryRepository transactionQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
    {
        _transactionQueryRepository = transactionQueryRepository;
    }

    public override async Task<PagedData<UserTransactionsResponseDto>> Handle(GetUserTransactionQuery query,
        CancellationToken cancellationToken)
    {
        var token = GetTokenFromHeader();
        var userDetails = await TokenService.GetUserByTokenAsync(new GetUserByTokenRequest { Token = token });
        var transactions = await _transactionQueryRepository.GetTransactionsByUserId(query, userDetails.Id);
        return transactions;
    }
}