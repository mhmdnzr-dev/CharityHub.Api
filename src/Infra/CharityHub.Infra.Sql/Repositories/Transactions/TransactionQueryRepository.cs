namespace CharityHub.Infra.Sql.Repositories.Transactions;

using Core.Contract.Primitives.Models;
using Core.Contract.Transactions.Queries;
using Core.Contract.Transactions.Queries.GetUserTransactions;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public class TransactionQueryRepository(
    CharityHubQueryDbContext queryDbContext,
    ILogger<TransactionQueryRepository> logger)
    : QueryRepository<Transaction>(queryDbContext), ITransactionQueryRepository
{
    public async Task<PagedData<UserTransactionsResponseDto>> GetTransactionsByUserId(GetUserTransactionQuery query,
        int userId)
    {
        // Log the start of the query
        logger.LogInformation("Starting GetTransactionsByUserId for UserId: {UserId}", userId);
        var totalCount = await _queryDbContext.Transactions.Where(t => t.UserId == userId).CountAsync();
        try
        {
            var transactions = await queryDbContext.Transactions
                .Where(t => t.UserId == userId) // Filtering by UserId
                .Include(t => t.Campaign)
                .OrderBy(t => t.CreatedAt) // Ensure ordering for consistent pagination
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(t => new UserTransactionsResponseDto
                {
                    CampaignName = t.Campaign.Title, // Assuming Campaign has a Title property
                    CharityName = t.Campaign.Charity.Name, // Assuming Charity is related to Campaign
                    AmountOfAssistance = t.Amount, // The Amount for the transaction
                    AssistanceDate = t.CreatedAt // Assuming there's a CreatedAt or similar field
                })
                .ToArrayAsync();

            // Log the successful retrieval of transactions
            logger.LogInformation("Successfully retrieved {TransactionCount} transactions for UserId: {UserId}",
                transactions.Length, userId);

            return new PagedData<UserTransactionsResponseDto>(transactions, totalCount, query.PageSize, query.Page);
        }
        catch (Exception ex)
        {
            // Log any error that occurs during the query
            logger.LogError(ex, "Error occurred while retrieving transactions for UserId: {UserId}", userId);
            throw; // Re-throw the exception to ensure the error is propagated
        }
    }
}