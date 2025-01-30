namespace CharityHub.Infra.Sql.Data.SeedData;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

using DbContexts;

using Microsoft.EntityFrameworkCore;

using Core.Domain.Entities;

public class DatabaseSeeder : ISeeder<CharityHubCommandDbContext>
{
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(ILogger<DatabaseSeeder> logger)
    {
        _logger = logger;
    }


    public async Task SeedAsync(CharityHubCommandDbContext context, CancellationToken cancellationToken = default)
    {
        if (!await context.Socials.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding social platforms...");

            var socials = new List<Social>
            {
                Social.Create("Facebook", "FB"),
                Social.Create("Twitter", "TW"),
                Social.Create("Instagram", "IG"),
                Social.Create("LinkedIn", "LI"),
                Social.Create("YouTube", "YT")
            };

            await context.Socials.AddRangeAsync(socials, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Social platforms seeding completed.");
        }

        if (!await context.Charities.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding charities...");
            
            var charities = new List<Charity>
            {
                Charity.Create(
                    name: "بنیاد کمک جهانی",
                    description: "کمک به مردم در سراسر جهان.",
                    website: "https://globalaid.org",
                    createdByUserId: 1,
                    address: "خیابان خیریه 123",
                    cityId: 100,
                    telephone: "+123456789",
                    managerName: "جان دو",
                    logoId: 10,
                    bannerId: 20,
                    socialId: 1,  // Example Social ID (make sure the ID exists in the Social table)
                    contactName: "جین اسمیت",
                    contactPhone: "+987654321"
                ),
                Charity.Create(
                    name: "اولویت آموزش",
                    description: "تامین منابع آموزشی برای کودکان محروم.",
                    website: "https://educationfirst.org",
                    createdByUserId: 2,
                    address: "خیابان دانش 456",
                    cityId: 200,
                    telephone: "+1122334455",
                    managerName: "آلیس جانسون",
                    logoId: 15,
                    bannerId: 25,
                    socialId: 2,  // Example Social ID (make sure the ID exists in the Social table)
                    contactName: "باب ویلیامز",
                    contactPhone: "+5544332211"
                )
            };

            await context.Charities.AddRangeAsync(charities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Charities seeding completed.");
        }
        
         
        if (!await context.Campaigns.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding campaigns...");

            var campaigns = new List<Campaign>
            {
                Campaign.Create(
                    title: "Help Orphans",
                    description: "Providing shelter and education for orphans.",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddMonths(6),
                    totalAmount: 50000m,
                    photoId: 4,
                    cityId: 110,
                    charityId: 1
                ),
                Campaign.Create(
                    title: "Food for Homeless",
                    description: "Providing meals for homeless people in the city.",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddMonths(3),
                    totalAmount: 20000m,
                    photoId: 5,
                    cityId: 100,
                    charityId: 2
                ),
                Campaign.Create(
                    title: "Education for Refugees",
                    description: "Ensuring education resources for displaced children.",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddMonths(12),
                    totalAmount: 75000m,
                    photoId: 8,
                    cityId: 120,
                    charityId: 1
                )
            };

            await context.Campaigns.AddRangeAsync(campaigns, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Campaigns seeding completed.");
        }

        if (!await context.Donations.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding donations...");

            var campaigns = await context.Campaigns.ToListAsync(cancellationToken);

            var donations = new List<Donation>
            {
                Donation.Create(userId: 1, campaignId: campaigns[0].Id, amount: 100m),
                Donation.Create(userId: 2, campaignId: campaigns[0].Id, amount: 200m),
                Donation.Create(userId: 3, campaignId: campaigns[1].Id, amount: 50m),
                Donation.Create(userId: 4, campaignId: campaigns[2].Id, amount: 500m),
                Donation.Create(userId: 5, campaignId: campaigns[2].Id, amount: 1000m)
            };

            await context.Donations.AddRangeAsync(donations, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Donations seeding completed.");
        }
        
        
        if (!await context.Categories.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding categories...");

            var categories = new List<Category>
            {
                Category.Create("Health"),
                Category.Create("Education"),
                Category.Create("Food"),
                Category.Create("Shelter")
            };

            await context.Categories.AddRangeAsync(categories, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Categories seeding completed.");
        }
        
       
        if (!await context.Transactions.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding transactions...");

            var users = await context.Users.ToListAsync(cancellationToken); // Fetch seeded users
            var campaigns = await context.Campaigns.ToListAsync(cancellationToken); // Fetch seeded campaigns

            var transactions = new List<Transaction>
            {
                // Sample transactions
                Transaction.Create(
                    userId: users[0].Id,  // Assuming user 1 exists
                    campaignId: campaigns[0].Id, // Assuming campaign 1 exists
                    amount: 100.00m
                ),
                Transaction.Create(
                    userId: users[1].Id,  // Assuming user 2 exists
                    campaignId: campaigns[1].Id, // Assuming campaign 2 exists
                    amount: 50.00m
                ),
                Transaction.Create(
                    userId: users[2].Id,  // Assuming user 3 exists
                    campaignId: campaigns[2].Id, // Assuming campaign 3 exists
                    amount: 200.00m
                )
            };

            await context.Transactions.AddRangeAsync(transactions, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transactions seeding completed.");
        }
    }
}