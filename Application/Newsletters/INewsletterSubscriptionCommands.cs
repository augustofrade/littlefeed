using LittleFeed.Common.Results;

namespace LittleFeed.Application.Newsletters;

public interface INewsletterSubscriptionCommands
{
    Task<Result> Subscribe(Guid newsletterId, string userId);
    Task<Result> SubscribeGuest(Guid newsletterId, string email);
    Task<Result> Unsubscribe(Guid newsletterId, string userId);
    Task<Result> UnsubscribeGuest(Guid newsletterId, string email);
}