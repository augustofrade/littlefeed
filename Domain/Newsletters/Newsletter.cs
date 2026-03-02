using LittleFeed.Domain.Common;

namespace LittleFeed.Domain.Newsletters;

public class Newsletter : Entity
{
    public string Slug { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? LastPostDate { get; set; }

    public ICollection<Article> Articles { get; private init; } = [];
    public ICollection<NewsletterMember> Members { get; private init; } = [];

    private Newsletter() { }

    public static Newsletter Create(string name, string description)
    {
        return new Newsletter
        {
            Name = name,
            Slug = Slugifier.Slugify(name),
            Description = description,
        };
    }

    /// <summary>
    /// Note: A Newsletter will always have an owner
    /// </summary>
    /// <returns></returns>
    public NewsletterMember GetOwner()
    {
        return Members.First(nm => nm.Role == NewsletterRole.Owner);
    }
    
    public bool CanBeEditedByUser(string memberUserId)
    {
        return Members.Any(nm => nm.UserId == memberUserId);
    }
}
