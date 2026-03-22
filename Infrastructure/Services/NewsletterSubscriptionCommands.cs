using LittleFeed.Application.Newsletters;
using LittleFeed.Common.Results;
using LittleFeed.Domain;
using LittleFeed.Domain.Newsletters;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Infrastructure.Services;

public class NewsletterSubscriptionCommands(INewsletterSubscriptionQueries subscriptionQueries,
    ApplicationDbContext dbContext) : INewsletterSubscriptionCommands
{
    public async Task<Result> Subscribe(Guid newsletterId, string userId)
    {
        if (await subscriptionQueries.IsUserSubscribed(userId))
            return Result.Success();
        
        var subscription = NewsletterSubscription.CreateForUser(newsletterId, userId);
        dbContext.NewsletterSubscriptions.Add(subscription);
        var results = await dbContext.SaveChangesAsync();
        return results > 0 ?  Result.Success() : Result.Failure("Failed to subscribe user to newsletter");
    }

    public async Task<Result> SubscribeGuest(Guid newsletterId, string email)
    {
        if (await subscriptionQueries.IsGuestSubscribed(email))
            return Result.Success();
        
        var subscription = NewsletterSubscription.CreateForGuest(newsletterId, email);
        dbContext.NewsletterSubscriptions.Add(subscription);
        var results = await dbContext.SaveChangesAsync();
        return results > 0 ?  Result.Success() : Result.Failure("Failed to subscribe email to newsletter");
    }

    public async Task<Result> Unsubscribe(Guid newsletterId, string userId)
    {
        var subscription = await dbContext.NewsletterSubscriptions
            .FirstOrDefaultAsync(ns => ns.UserId == userId && ns.NewsletterId == newsletterId);
        
        if(subscription == null)
            return Result.Failure("No subscription found for user");
        
        subscription.Unsubscribe();
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UnsubscribeGuest(Guid newsletterId, string email)
    {
        var subscription = await dbContext.NewsletterSubscriptions
            .FirstOrDefaultAsync(ns => ns.GuestEmail == email && ns.NewsletterId == newsletterId);
        
        if(subscription == null)
            return Result.Failure("No subscription found for provided email");
        
        subscription.Unsubscribe();
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }
}