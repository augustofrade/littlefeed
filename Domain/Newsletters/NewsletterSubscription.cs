namespace LittleFeed.Domain.Newsletters;

public class NewsletterSubscription
{
    public Guid Id { get; private init; } =  Guid.NewGuid();
    public string? UserId { get; private init; }
    public string? GuestEmail { get; private init; }
    public DateTime SubscribedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UnsubscribedAt { get; private set; }
    public Guid NewsletterId { get; private init; }
    public Newsletter Newsletter { get; private set; }
    
    public bool IsSubscribed => UnsubscribedAt == null;
    
    public bool IsGuestSubscribed => GuestEmail != null && UserId == null;

    private NewsletterSubscription() { }
    
    private NewsletterSubscription(Guid newsletterId, string userId)
    {
        NewsletterId = newsletterId;
        UserId = userId;
    }

    public static NewsletterSubscription CreateForUser(Guid newsletterId, string userId)
    {
        return new NewsletterSubscription
        {
            NewsletterId = newsletterId,
            UserId = userId,
            GuestEmail = null
        };
    }
    
    public static NewsletterSubscription CreateForGuest(Guid newsletterId, string guestEmail)
    {
        return new NewsletterSubscription
        {
            NewsletterId = newsletterId,
            UserId = null,
            GuestEmail = guestEmail
        };
    }

    public void Unsubscribe()
    {
        if(!IsSubscribed) return;

        UnsubscribedAt = DateTime.UtcNow;
    }
    
    public void Resubscribe()
    {
        if (IsSubscribed) return;
        
        SubscribedAt = DateTime.UtcNow;
        UnsubscribedAt = null;
    }
}