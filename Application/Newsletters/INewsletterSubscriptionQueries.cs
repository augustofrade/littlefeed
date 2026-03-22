using LittleFeed.Dto.NewsletterSubscriptions;
using LittleFeed.Infrastructure.Identity;

namespace LittleFeed.Application.Newsletters;

public interface INewsletterSubscriptionQueries
{
    Task<bool> IsUserSubscribed(string userId);
    Task<bool> IsGuestSubscribed(string email);
    Task<bool> IsEmailSubscribed(string email);
    Task<List<string>> GetAllActiveEmailSubscriptionsForNewsletter(Guid newsletterId);
    Task<List<ListNewsletterSubscriptionDto>> GetSubscriptionsForNewsletter(Guid newsletterId);
    Task<List<ListUserNewsletterSubscriptionDto>> GetAllSubscriptionsForUser(string userId);
    Task<List<ListUserActiveNewsletterSubscriptionDto>> GetActiveSubscriptionsForUser(string userId);
}