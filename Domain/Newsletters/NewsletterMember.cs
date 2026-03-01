namespace LittleFeed.Domain.Newsletters;

public class NewsletterMember : Entity
{
    public Guid NewsletterId { get; private set; }
    public required string UserId { get; set; }
    public required NewsletterRole Role { get; set; }
    
    private NewsletterMember() { }

    public static NewsletterMember CreateOwner(Guid newsletterId, string userId)
    {
        return new NewsletterMember
        {
            NewsletterId = newsletterId,
            UserId = userId,
            Role = NewsletterRole.Owner
        };
    }
    
    public static NewsletterMember CreateWriter(Guid newsletterId, string userId)
    {
        return new NewsletterMember
        {
            NewsletterId = newsletterId,
            UserId = userId,
            Role = NewsletterRole.Writer
        };
    }
}