using Humanizer;
using LittleFeed.Domain.Common;

namespace LittleFeed.Domain.Newsletters;

public class Article : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public required string Excerpt { get; set; }
    public required string Body { get; set; }
    public required string AuthorId { get; init; }
    public DateTime? PublishDate { get; private set; }
    public Guid NewsletterId { get; private set; }
    public Newsletter Newsletter { get; private set; }
    public ICollection<ArticleComment> Comments { get; init; } = [];

    private Article() { }

    public static Article Create(string title, string body, string authorId, Guid newsletterId)
    {
        return new Article
        {
            Title = title,
            Slug = Slugifier.Slugify(title),
            Excerpt = body.Truncate(100),
            Body = body,
            NewsletterId = newsletterId,
            AuthorId = authorId,
        };
    }
    
    public bool IsDraft => PublishDate == null;

    public void SetSlug(string slug)
    {
        Slug = Slugifier.Slugify(slug);
    }

    public void Publish()
    {
        if (PublishDate != null) return;
        
        PublishDate = DateTime.UtcNow;
    }
}
