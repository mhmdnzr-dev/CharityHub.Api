namespace CharityHub.Core.Contract.Transactions.Queries;

using System.Collections;

using Domain.Entities;

using GetUserTransactions;

using Primitives.Models;
using Primitives.Repositories;

public interface ITransactionQueryRepository : IQueryRepository<Transaction>
{
    Task<PagedData<UserTransactionsResponseDto>> GetTransactionsByUserId(GetUserTransactionQuery query, int userId);
}