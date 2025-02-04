namespace CharityHub.Core.Contract.Transactions.Queries;

using System.Collections;

using Domain.Entities;

using GetUserTransactions;

using Primitives.Repositories;

public interface ITransactionQueryRepository : IQueryRepository<Transaction>
{
    Task<IEnumerable<UserTransactionsResponseDto>> GetTransactionsByUserId(GetUserTransactionQuery query, int userId);
}