using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Dto.NewsletterSubscriptions;

public record ListUserNewsletterSubscriptionDto(
    NewsletterIdentificationDto Newsletter,
    DateTime SubscriptionDate,
    DateTime? SubscriptionEndDate);