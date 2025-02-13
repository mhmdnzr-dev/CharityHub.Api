namespace CharityHub.Infra.Sql.Data.SeedData;

using System.Threading;
using System.Threading.Tasks;

using Core.Domain.Entities;
using Core.Domain.Entities.Identity;

using DbContexts;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DatabaseSeeder : ISeeder<CharityHubCommandDbContext>
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public DatabaseSeeder(ILogger<DatabaseSeeder> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }


    public async Task SeedAsync(CharityHubCommandDbContext context, CancellationToken cancellationToken = default)
    {
        if (!await context.Users.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding users...");

            // Create sample users
            List<ApplicationUser> users = new()
            {
                new()
                {
                    UserName = "09203216120",
                    Email = "john.doe@example.com",
                    FirstName = "test",
                    LastName = "testy",
                    PhoneNumber = "09203216120",
                },
                new()
                {
                    UserName = "09203216121",
                    Email = "jane.smith@example.com",
                    FirstName = "test",
                    LastName = "testy",
                    PhoneNumber = "09203216121"
                },
                new()
                {
                    UserName = "09203216130",
                    Email = "admin@example.com",
                    FirstName = "test",
                    LastName = "testy",
                    PhoneNumber = "09203216130"
                }
            };

            // Create users with UserManager (which hashes the password)
            foreach (var user in users)
            {
                if (!user.IsActive)
                {
                    user.Activate();
                }

                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result =
                        await _userManager.CreateAsync(user,
                            "Password123!"); // You can change the default password here
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"User {user.UserName} created.");
                    }
                    else
                    {
                        _logger.LogError(
                            $"Error creating user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }

                    if (!user.IsActive)
                    {
                        user.Activate();
                    }
                }
            }

            _logger.LogInformation("Users seeding completed.");
        }

        if (!await context.Socials.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding social platforms...");

            var socials = new List<Social>
            {
                Social.Create("Facebook", "FB", "https://facebook.com"),
                Social.Create("Twitter", "TW", "https://twitter.com"),
                Social.Create("Instagram", "IG", "https://instagram.com"),
                Social.Create("LinkedIn", "LI", "https://linkedin.com"),
                Social.Create("YouTube", "YT", "https://youtube.com")
            };

            await context.Socials.AddRangeAsync(socials, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Social platforms seeding completed.");
        }

        if (!await context.Charities.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding charities...");
            var socials = await context.Socials.ToListAsync(cancellationToken); // Fetch seeded campaigns
            var users = await context.ApplicationUsers.ToListAsync(cancellationToken); // Fetch seeded campaigns

            var charities = new List<Charity>
            {
                Charity.Create(
                    name: "بنیاد کمک جهانی",
                    description: "کمک به مردم در سراسر جهان.",
                    website: "https://globalaid.org",
                    createdByUserId: users[0].Id,
                    address: "خیابان خیریه 123",
                    cityId: 1,
                    telephone: "+123456789",
                    managerName: "جان دو",
                    socialId: socials[0].Id, // Example Social ID (make sure the ID exists in the Social table)
                    contactName: "جین اسمیت",
                    contactPhone: "+987654321"
                ),
                Charity.Create(
                    name: "اولویت آموزش",
                    description: "تامین منابع آموزشی برای کودکان محروم.",
                    website: "https://educationfirst.org",
                    createdByUserId: users[1].Id,
                    address: "خیابان دانش 456",
                    cityId: 1,
                    telephone: "+1122334455",
                    managerName: "آلیس جانسون",
                    socialId: socials[1].Id, // Example Social ID (make sure the ID exists in the Social table)
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
                    title: "کمک به یتیمان",
                    description: "تأمین پناهگاه و آموزش برای یتیمان.",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddMonths(6),
                    totalAmount: 50000m,
                    photoId: 4,
                    cityId: 110,
                    charityId: 1
                ),
                Campaign.Create(
                    title: "غذا برای بی خانمان ها",
                    description: "تأمین وعده های غذایی برای افراد بی خانمان در شهر.",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddMonths(3),
                    totalAmount: 20000m,
                    photoId: 5,
                    cityId: 100,
                    charityId: 2
                ),
                Campaign.Create(
                    title: "آموزش برای پناهندگان",
                    description: "تأمین منابع آموزشی برای کودکان آواره.",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddMonths(12),
                    totalAmount: 75000m,
                    photoId: 8,
                    cityId: 120,
                    charityId: 1
                ),
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
                Donation.Create(userId: 1, campaignId: campaigns[1].Id, amount: 50m),
                Donation.Create(userId: 1, campaignId: campaigns[2].Id, amount: 500m),
                Donation.Create(userId: 2, campaignId: campaigns[2].Id, amount: 1000m)
            };

            foreach (var donation in donations)
            {
                var campaign = campaigns.FirstOrDefault(c => c.Id == donation.CampaignId);
                if (campaign is not null)
                {
                    campaign.AddDonation(donation); // Assuming AddDonation is a method on Campaign
                }
            }

            await context.Donations.AddRangeAsync(donations, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Donations seeding completed.");
        }


        if (!await context.Categories.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding categories...");

            var categories = new List<Category>
            {
                Category.Create("سلامت"),
                Category.Create("آموزش"),
                Category.Create("غذا"),
                Category.Create("پناهگاه")
            };


            await context.Categories.AddRangeAsync(categories, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Categories seeding completed.");
        }

        if (!await context.Terms.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding terms...");

            var terms = new List<Term>
            {
                Term.Create("شرایط استفاده"), Term.Create("خط مشی حریم خصوصی"), Term.Create("قوانین و مقررات")
            };

            await context.Terms.AddRangeAsync(terms, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Terms seeding completed.");
        }

        if (!context.ApplicationUserTerms.Any())
        {
            var user = context.ApplicationUsers.ToList();
            var terms = context.Terms.OrderByDescending(data => data.CreatedAt).FirstOrDefault();

            var userTerms = new List<ApplicationUserTerm>
            {
                ApplicationUserTerm.Create(user[0].Id, terms.Id),
                ApplicationUserTerm.Create(user[1].Id, terms.Id),
                ApplicationUserTerm.Create(user[2].Id, terms.Id)
            };
            await context.ApplicationUserTerms.AddRangeAsync(userTerms, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
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
                    userId: users[0].Id, // Assuming user 1 exists
                    campaignId: campaigns[0].Id, // Assuming campaign 1 exists
                    amount: 100.00m
                ),
                Transaction.Create(
                    userId: users[1].Id, // Assuming user 2 exists
                    campaignId: campaigns[1].Id, // Assuming campaign 2 exists
                    amount: 50.00m
                ),
                Transaction.Create(
                    userId: users[2].Id, // Assuming user 3 exists
                    campaignId: campaigns[2].Id, // Assuming campaign 3 exists
                    amount: 200.00m
                )
            };

            await context.Transactions.AddRangeAsync(transactions, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transactions seeding completed.");
        }

        if (!await context.CampaignCategories.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding campaign categories...");

            var categories = await context.Categories.ToListAsync(cancellationToken);
            var campaigns = await context.Campaigns.ToListAsync(cancellationToken);

            if (campaigns.Count < 3 || categories.Count < 4)
            {
                _logger.LogError("Not enough campaigns or categories to seed campaign categories.");
                return;
            }

            var campaignCategories = new List<CampaignCategory>
            {
                CampaignCategory.Create(campaigns[0].Id, categories[3].Id),
                CampaignCategory.Create(campaigns[0].Id, categories[1].Id),
                CampaignCategory.Create(campaigns[1].Id, categories[2].Id),
                CampaignCategory.Create(campaigns[2].Id, categories[1].Id)
            };

            await context.CampaignCategories.AddRangeAsync(campaignCategories, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Campaign categories seeding completed.");
        }
    }
}