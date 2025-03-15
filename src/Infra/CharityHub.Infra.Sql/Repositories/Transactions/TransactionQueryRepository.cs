namespace CharityHub.Infra.Sql.Repositories.Transactions;

using Core.Contract.Features.Transactions.Queries;
using Core.Contract.Features.Transactions.Queries.GetUserTransactions;
using Core.Contract.Primitives.Models;
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

        try
        {
            var transactionsQuery = from transaction in _queryDbContext.Transactions
                where transaction.UserId == userId
                orderby transaction.CreatedAt
                join bannerFile in _queryDbContext.StoredFiles
                    on transaction.Campaign.BannerId equals bannerFile.Id into bannerFileGroup
                from bannerFile in bannerFileGroup.DefaultIfEmpty()
                join charityLogoFile in _queryDbContext.StoredFiles
                    on transaction.Campaign.Charity.LogoId equals charityLogoFile.Id into charityLogoFileGroup
                from charityLogoFile in charityLogoFileGroup.DefaultIfEmpty()
                select new UserTransactionsResponseDto
                {
                    CampaignName = transaction.Campaign.Title,
                    CharityId = transaction.Campaign.CharityId,
                    CharityName = transaction.Campaign.Charity.Name,
                    CharityLogoUrl = charityLogoFile != null
                        ? charityLogoFile.FilePath.Replace("\\", "/")
                        : "/uploads/default-charity.png",
                    AmountOfAssistance = transaction.Amount,
                    AssistanceDate = transaction.CreatedAt,
                    CampaignBannerPhotoUrlAddress = bannerFile != null
                        ? bannerFile.FilePath.Replace("\\", "/")
                        : "/uploads/default-campaign.png"
                };

            var totalCount = await _queryDbContext.Transactions
                .Where(t => t.UserId == userId)
                .CountAsync();

            var transactions = await transactionsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArrayAsync();

            // Log successful retrieval
            logger.LogInformation("Successfully retrieved {TransactionCount} transactions for UserId: {UserId}",
                transactions.Length, userId);

            return new PagedData<UserTransactionsResponseDto>(transactions, totalCount, query.PageSize, query.Page);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving transactions for UserId: {UserId}", userId);
            throw;
        }
    }
}