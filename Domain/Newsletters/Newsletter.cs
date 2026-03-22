using LittleFeed.Domain.Common;

namespace LittleFeed.Domain.Newsletters;

public class Newsletter : Entity
{
    public string Slug { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required string OwnerId { get; set; }
    public DateTime? LastPostDate { get; set; }

    public ICollection<Article> Articles { get; private init; } = [];
    public ICollection<NewsletterMember> Members { get; private init; } = [];
    public ICollection<NewsletterSubscription> Subscriptions { get; private init; } = [];

    private Newsletter() { }

    public static Newsletter Create(string name, string description, string ownerId)
    {
        return new Newsletter
        {
            Name = name,
            Slug = Slugifier.Slugify(name),
            Description = description,
            OwnerId = ownerId,
        };
    }

    public bool IsOwner(string ownerId)
    {
        return OwnerId == ownerId;
    }

    public IEnumerable<NewsletterMember> GetWriters(string ownerId)
    {
        return  Members.Where(m => m.UserId == ownerId && m.Role != NewsletterRole.Writer);
    }
    
    public bool CanBeEditedByUser(string memberUserId)
    {
        return OwnerId == memberUserId || Members.Any(nm => nm.UserId == memberUserId);
    }
}
