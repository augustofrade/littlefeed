using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Dto.NewsletterSubscriptions;

public record ListUserActiveNewsletterSubscriptionDto(
    NewsletterIdentificationDto Newsletter,
    DateTime SubscriptionDate);