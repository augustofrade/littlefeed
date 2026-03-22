using LittleFeed.Application.Newsletters;
using LittleFeed.Domain;
using LittleFeed.Dto.Newsletters;
using LittleFeed.Dto.NewsletterSubscriptions;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Infrastructure.Services;

public class NewsletterSubscriptionQueries(ApplicationDbContext dbContext) : INewsletterSubscriptionQueries
{
    public Task<bool> IsUserSubscribed(string userId)
    {
        return dbContext.NewsletterSubscriptions.AnyAsync(ns => ns.UserId == userId && ns.UnsubscribedAt == null);
    }

    public Task<bool> IsGuestSubscribed(string email)
    {
        return dbContext.NewsletterSubscriptions.AnyAsync(ns => ns.GuestEmail == email && ns.UnsubscribedAt == null);
    }

    public async Task<bool> IsEmailSubscribed(string email)
    {
        var userId = await dbContext.Users
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleOrDefaultAsync();

        return await dbContext.NewsletterSubscriptions
            .AnyAsync(ns => ns.UnsubscribedAt == null && ns.GuestEmail == email ||
                        (userId != null && ns.UserId == userId));
    }

    public async Task<List<string>> GetAllActiveEmailSubscriptionsForNewsletter(Guid newsletterId)
    {
        var emails = await dbContext.NewsletterSubscriptions
            .Where(ns => ns.NewsletterId == newsletterId && ns.UnsubscribedAt == null)
            .Select(ns => ns.GuestEmail ?? dbContext.Users
                .Where(u => u.Id == ns.UserId)
                .Select(u => u.Email)
                .FirstOrDefault())
            .Where(email => email != null)
            .Distinct()
            .ToListAsync();

        return emails!;
    }

    public async Task<List<ListNewsletterSubscriptionDto>> GetSubscriptionsForNewsletter(Guid newsletterId)
    {
        var subscriptions = await dbContext.NewsletterSubscriptions
            .Where(ns => ns.NewsletterId == newsletterId)
            .Select(ns => ns.GuestEmail != null
                    ? new ListNewsletterSubscriptionDto(null, ns.SubscribedAt, ns.UnsubscribedAt)
                    : dbContext.UserProfiles
                    .Where(u => u.UserId == ns.UserId)
                    .Select(u => new ListNewsletterSubscriptionDto(u.DisplayName, ns.SubscribedAt, ns.UnsubscribedAt))
                    .FirstOrDefault())
            .Where(email => email != null)
            .ToListAsync();

        return subscriptions!;
    }

    public async Task<List<ListUserNewsletterSubscriptionDto>> GetAllSubscriptionsForUser(string userId)
    {
        var subscriptions = await dbContext.NewsletterSubscriptions
            .Where(ns => ns.UserId == userId)
            .Join(
                dbContext.Newsletters,
                ns => ns.NewsletterId,
                n => n.Id,
                (ns, n) => new ListUserNewsletterSubscriptionDto(
                    Newsletter: new NewsletterIdentificationDto(n.Name, n.Slug),
                    SubscriptionDate: ns.SubscribedAt,
                    SubscriptionEndDate: ns.UnsubscribedAt
                )
            )
            .ToListAsync();

        return subscriptions;
    }

    public async Task<List<ListUserActiveNewsletterSubscriptionDto>> GetActiveSubscriptionsForUser(string userId)
    {
        var subscriptions = await dbContext.NewsletterSubscriptions
            .Where(ns => ns.UserId == userId)
            .Join(
                dbContext.Newsletters,
                ns => ns.NewsletterId,
                n => n.Id,
                (ns, n) => new ListUserActiveNewsletterSubscriptionDto(
                    Newsletter: new NewsletterIdentificationDto(n.Name, n.Slug),
                    SubscriptionDate: ns.SubscribedAt
                )
            )
            .ToListAsync();

        return subscriptions;
    }
}