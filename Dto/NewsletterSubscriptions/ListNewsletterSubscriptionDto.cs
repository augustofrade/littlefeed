namespace LittleFeed.Dto.NewsletterSubscriptions;

public sealed record ListNewsletterSubscriptionDto(
    string? UserName,
    DateTime SubscriptionDate,
    DateTime? SubscriptionEndDate);